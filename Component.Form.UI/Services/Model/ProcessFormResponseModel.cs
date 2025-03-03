using System;
using Component.Form.Model;

namespace Component.Form.UI.Services.Model;

public class ProcessFormResponseModel
{
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    public string NextPage { get; set; }
    public Dictionary<string, string> FormData { get; set; } = new Dictionary<string, string>();
}
