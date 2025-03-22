using System;
using Microsoft.AspNetCore.Http;

namespace Component.Files.Application.UploadFile.Model;

public class UploadFileRequest
{
    public IFormFileCollection Files { get; set; }
}
