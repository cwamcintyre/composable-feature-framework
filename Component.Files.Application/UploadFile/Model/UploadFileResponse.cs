using System;
using Component.Files.Model;

namespace Component.Files.Application.UploadFile.Model;

public class UploadFileResponse
{
    public string FileUrl { get; set; } = string.Empty;
    public UploadResult UploadResult { get; set; }
    public ScanResult ScanResult { get; set; }
}
