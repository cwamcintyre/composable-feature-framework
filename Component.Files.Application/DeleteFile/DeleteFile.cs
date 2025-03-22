using System;
using Component.Core.Application;
using Component.Files.Application.DeleteFile.Model;
using Component.Files.Application.Shared.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Component.Files.Application.DeleteFile;

public class DeleteFile : IRequestUseCase<DeleteFileRequest>
{
    private readonly ILogger<DeleteFile> _logger;
    private readonly IFileStore _fileStore;

    public DeleteFile(IFileStore fileStore, ILogger<DeleteFile> logger)
    {
        _logger = logger;
        _fileStore = fileStore;
    }
    
    public async Task HandleAsync(DeleteFileRequest request)
    {
        await _fileStore.DeleteFileAsync(request.FileName);
        _logger.LogInformation($"File {request.FileName} deleted from file store.");
    }
}