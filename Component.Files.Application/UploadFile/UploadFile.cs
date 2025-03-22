using System;
using Component.Core.Application;
using Component.Files.Application.Shared.Infrastructure;
using Component.Files.Application.UploadFile.Model;
using Component.Files.Model;
using Polly;

namespace Component.Files.Application.UploadFile;

public class UploadFile : IRequestResponseUseCase<UploadFileRequest, UploadFileResponse>
{
    private readonly IFileStore _fileStore;
    private readonly IVirusScanner _virusScanner;

    public UploadFile(IFileStore fileStore, IVirusScanner virusScanner)
    {
        _fileStore = fileStore;
        _virusScanner = virusScanner;
    }

    public async Task<UploadFileResponse> HandleAsync(UploadFileRequest request)
    {
        if (request.Files.Count == 0)
        {
            throw new ArgumentException("No files to upload.");
        }

        var file = request.Files[0];
        var fileName = file.FileName;
        var fileStream = file.OpenReadStream();

        // Assuming you have a file store service injected
        var uploadResult = await _fileStore.UploadFileAsync(fileName, fileStream);

        // Define a retry policy with Polly
        var retryPolicy = Policy
            .HandleResult<ScanResult>(result => result.Status == ScanResult.Unknown)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(20));

        // Execute the scan with retry logic
        var scanResult = await retryPolicy.ExecuteAsync(async () =>
        {
            return await _virusScanner.ScanFileAsync(fileName);
        });

        switch (scanResult.Status)
        {
            case ScanResult.Clean:
                // File is clean, proceed with the upload
                break;
            case ScanResult.Malicious:
                // Handle infected file scenario (e.g., delete the uploaded file)
                await _fileStore.DeleteFileAsync(fileName);
                throw new InvalidOperationException("File is infected and has been deleted.");            
        }

        return new UploadFileResponse
        {
            UploadResult = uploadResult,
            ScanResult = scanResult
        };
    }
}
