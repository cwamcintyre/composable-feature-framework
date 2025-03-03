using System;
using Component.Form.Model;

namespace Component.Form.Application.UseCase.ProcessForm.Model;

public class ProcessFormResponseModel
{
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    public string NextPage { get; set; }
    public Dictionary<string, string> FormData { get; set; } = new Dictionary<string, string>();
    public Stack<string> FormRoute { get; set; } = new Stack<string>();
}
