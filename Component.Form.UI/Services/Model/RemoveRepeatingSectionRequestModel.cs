namespace Component.Form.UI.Services.Model;

public class RemoveRepeatingSectionRequestModel
{
    public string FormId { get; set; }
    public string PageId { get; set; }
    public string ApplicantId { get; set; }
    public int Index { get; set; }
}