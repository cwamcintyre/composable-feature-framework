using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Component.Core.Application;
using Component.Files.Application.GetFile.Model;
using Component.Files.Application.Shared.Infrastructure;

namespace Component.Files.Application.GetFile;

public class GetFile : IRequestResponseUseCase<GetFileRequest, GetFileResponse>
{
    private readonly IFileStore _fileStore;

    public GetFile(IFileStore fileStore)
    {
        _fileStore = fileStore;
    }

    public async Task<GetFileResponse> HandleAsync(GetFileRequest request)
    {
        var fileStream = await _fileStore.GetFileAsync(request.FileName, CancellationToken.None);
        return new GetFileResponse
        {
            Stream = fileStream
        };
    }
}
