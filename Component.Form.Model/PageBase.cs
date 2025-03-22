using System;
using Component.Form.Model;
namespace Component.Form.Model;

public class PageBase
{
    public string PageId { get; set; }
    public string PageType { get; set; }
    public string Title { get; set; }
    public List<Model.Component> Components { get; set; }
    public List<Condition> Conditions { get; set; }
    public Dictionary<string, string> Data { get; set; }
    public string NextPageId { get; set; }
}
