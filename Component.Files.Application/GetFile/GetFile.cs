using System;
using Component.Core.Application;
using Component.Files.Application.GetFile.Model;

namespace Component.Files.Application.GetFile;

public class GetFile : IRequestResponseUseCase<GetFileRequest, GetFileResponse>
{
    public Task<GetFileResponse> HandleAsync(GetFileRequest request)
    {
        throw new NotImplementedException();
    }
}
