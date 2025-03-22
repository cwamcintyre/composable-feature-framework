namespace Component.Files.Model;

public class ScanResult
{
    public const string Clean = "No threats found";
    public const string Malicious = "Malicious";
    public const string Error = "Error";
    public const string Unknown = "Unknown";

    public string FileName { get; set; }
    public bool IsClean { get; set; }
    public string Status { get; set; } 
    public string Message { get; set; }
}
