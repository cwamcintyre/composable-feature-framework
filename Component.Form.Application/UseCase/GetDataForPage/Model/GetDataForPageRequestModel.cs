using System;

namespace Component.Form.Application.UseCase.GetDataForPage.Model;

public class GetDataForPageRequestModel
{
    public string FormId { get; set; }
    public string ApplicantId { get; set; }
    public string PageId { get; set; }  
    public int RepeatIndex { get; set; }
}
