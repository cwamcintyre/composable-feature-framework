using Component.Files.Application.Shared.Infrastructure;
using Component.Files.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Azure;
using Microsoft.Extensions.Logging;

public class BlobFileStore : IFileStore
{
    private readonly ILogger<BlobFileStore> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobFileStore(BlobServiceClient blobServiceClient, IConfiguration configuration, ILogger<BlobFileStore> logger)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["BlobStorage:ContainerName"];
    }

    public async Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
        _logger.LogInformation($"File {fileName} deleted from blob storage.");
    }

    public async Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        return await blobClient.ExistsAsync(cancellationToken);
    }

    public async Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        var response = await blobClient.DownloadAsync(cancellationToken);
        return response.Value.Content;
    }

    public async Task<UploadResult> UploadFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);

        _logger.LogInformation($"File {fileName} uploaded to blob storage {_containerName}");

        return new UploadResult
        {
            Success = true
        };
    }

    public async Task MoveToQuarantineAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var quarantineContainerClient = _blobServiceClient.GetBlobContainerClient("quarantine");
        await quarantineContainerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var quarantineBlobClient = quarantineContainerClient.GetBlobClient(fileName);
        await quarantineBlobClient.SyncCopyFromUriAsync(blobClient.Uri, cancellationToken: cancellationToken);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);

        _logger.LogInformation($"File {fileName} moved to quarantine.");
    }
}