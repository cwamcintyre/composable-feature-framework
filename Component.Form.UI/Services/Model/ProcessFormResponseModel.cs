using System;
using Component.Form.Model;

namespace Component.Form.UI.Services.Model;

public class ProcessFormResponseModel
{
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
    public string NextPage { get; set; }
    public string FormData { get; set; }
    public int RepeatIndex { get; set; }
}
