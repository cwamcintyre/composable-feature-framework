using System;

namespace Component.Form.UI.Services.Model;

public class ProcessFormRequestModel
{
    public string FormId { get; set; }
    public string PageId { get; set; }
    public string ApplicantId { get; set; }
    public Dictionary<string, string> Form { get; set; }
}
