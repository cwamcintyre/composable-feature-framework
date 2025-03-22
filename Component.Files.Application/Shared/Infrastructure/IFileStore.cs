using System;
using Component.Files.Model;

namespace Component.Files.Application.Shared.Infrastructure;

public interface IFileStore
{
    Task<UploadResult> UploadFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default);
    Task MoveToQuarantineAsync(string fileName, CancellationToken cancellationToken = default);
}
