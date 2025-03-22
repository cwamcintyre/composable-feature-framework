using System;
using Component.Core.Application;
using Component.Files.Application.DeleteFile.Model;

namespace Component.Files.Application.DeleteFile;

public class DeleteFile : IRequestResponseUseCase<DeleteFileRequest, DeleteFileResponse>
{
    public Task<DeleteFileResponse> HandleAsync(DeleteFileRequest request)
    {
        throw new NotImplementedException();
    }
}