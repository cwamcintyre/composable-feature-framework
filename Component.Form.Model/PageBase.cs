using System;
using Component.Form.Model;
namespace Component.Form.Model;

public class PageBase
{
    public required string PageId { get; set; }
    public required string PageType { get; set; }
    public required string Title { get; set; }
    public required List<Model.Component> Components { get; set; }
    public List<Condition> Conditions { get; set; }
    public Dictionary<string, string> Data { get; set; }
    public required string NextPageId { get; set; }
}
