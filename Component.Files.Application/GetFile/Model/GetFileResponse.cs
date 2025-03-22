using System;

namespace Component.Files.Application.GetFile.Model;

public class GetFileResponse
{
    public Stream Stream { get; set; } = Stream.Null;
}
