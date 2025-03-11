using System;
using Component.Form.Model;

namespace Component.Form.Application.UseCase.ProcessForm.Model;

public class ProcessFormResponseModel
{
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
    public int RepeatIndex { get; set; }
    public string NextPage { get; set; }
    public string FormData { get; set; }
    public Stack<string> FormRoute { get; set; } = new Stack<string>();
}
