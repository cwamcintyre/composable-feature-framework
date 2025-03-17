using System;
using Component.Form.Model;

namespace Component.Form.UI.Services.Model;

public class ProcessFormResponseModel
{
    public Dictionary<string, List<string>> Errors { get; set; }
    public string NextPageId { get; set; }
    public string ExtraData { get; set; }
}