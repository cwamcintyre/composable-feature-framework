using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Component.Files.Application.Shared.Infrastructure;
using Component.Files.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Component.Files.Infrastructure.Blob;

public class AzureOnUploadVirusScanner : IVirusScanner
{
    public static string VirusScanTagKey = "Malware Scanning scan result";

    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private ILogger<AzureOnUploadVirusScanner> _logger;

    public AzureOnUploadVirusScanner(BlobServiceClient blobServiceClient, IConfiguration configuration, ILogger<AzureOnUploadVirusScanner> logger)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["BlobStorage:ContainerName"];
    }

    public async Task<ScanResult> ScanFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        
        // Use BlobBaseClient to fetch index tags
        BlobBaseClient baseClient = blobClient.WithSnapshot(null);

        try
        {
            var tagResult = await baseClient.GetTagsAsync();
            IDictionary<string, string> tags = tagResult.Value.Tags;

            if (tags.ContainsKey(VirusScanTagKey))
            {
                if (tags[VirusScanTagKey] == ScanResult.Clean)
                {
                    return new ScanResult() 
                    {
                        FileName = fileName,
                        IsClean = tags[VirusScanTagKey] == ScanResult.Clean,
                        Status = tags[VirusScanTagKey]
                    };
                }
                else if (tags[VirusScanTagKey] == ScanResult.Malicious)
                {
                    return new ScanResult() 
                    {
                        FileName = fileName,
                        IsClean = false,
                        Status = ScanResult.Malicious,
                        Message = "The file is infected with a virus."
                    };
                }
                else 
                {
                    var status = tags[VirusScanTagKey];
                    var newMessage = "";
                    
                    if (status.StartsWith("SAM259210"))
                    {
                        newMessage = "The file is password protected and cannot be scanned for viruses. Upload a file that is not password protected.";
                        _logger.LogInformation($"File {fileName} is password protected. User should upload a file that is not password protected.");
                    }

                    if (status.StartsWith("SAM259201") || 
                        status.StartsWith("SAM259207") ||
                        status.StartsWith("SAM259212"))
                    {
                        newMessage = "There was a problem scanning the file for viruses. Try uploading again.";
                        _logger.LogInformation($"Transient error scanning file {fileName} for viruses. Error code: {status} . User should try again.");
                    }

                    if (status.StartsWith("SAM259213") ||
                        status.StartsWith("SAM259211") ||
                        status.StartsWith("SAM259209") ||
                        status.StartsWith("SAM259208") ||
                        status.StartsWith("SAM259205") ||
                        status.StartsWith("SAM259204") ||
                        status.StartsWith("SAM259203"))
                    {
                        newMessage = "There was an error scanning the file for viruses. The problem has been reported to the IT team.";
                        _logger.LogError($"Error scanning file {fileName} for viruses. Error code: {status}");
                    }

                    return new ScanResult() 
                    {
                        FileName = fileName,
                        IsClean = false,
                        Status = ScanResult.Error,
                        Message = newMessage
                    };
                }
            }
            else
            {
                return new ScanResult() 
                {
                    FileName = fileName,
                    IsClean = false,
                    Status = ScanResult.Unknown
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving blob tags for file {fileName}", ex.Message);
            return new ScanResult() 
            {
                FileName = fileName,
                IsClean = false,
                Status = ScanResult.Error,
                Message = "There was an unknown error. The problem has been reported to the IT team."
            };
        }
    }
}
