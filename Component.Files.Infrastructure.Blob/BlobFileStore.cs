using Component.Files.Application.Shared.Infrastructure;
using Component.Files.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class BlobFileStore : IFileStore
{
    private readonly ILogger<BlobFileStore> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly string _quarantineName;

    public BlobFileStore(BlobServiceClient blobServiceClient, IConfiguration configuration, ILogger<BlobFileStore> logger)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["BlobStorage:ContainerName"];
        _quarantineName = configuration["BlobStorage:QuarantineContainerName"];
    }

    public async Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }

    public async Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        return await blobClient.ExistsAsync(cancellationToken);
    }

    public async Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try 
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            return await blobClient.OpenReadAsync(new BlobOpenReadOptions(false), cancellationToken);
        }
        catch (Azure.RequestFailedException ex) when (ex.ErrorCode == "BlobNotFound")
        {
            throw new FileNotFoundException($"The file '{fileName}' was not found.");
        }
    }

    public async Task<UploadResult> UploadFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);

        return new UploadResult
        {
            Success = true
        };
    }

    public async Task MoveToQuarantineAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var quarantineContainerClient = _blobServiceClient.GetBlobContainerClient(_quarantineName);
        await quarantineContainerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var quarantineBlobClient = quarantineContainerClient.GetBlobClient(fileName);
        await quarantineBlobClient.SyncCopyFromUriAsync(blobClient.Uri, cancellationToken: cancellationToken);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }
}