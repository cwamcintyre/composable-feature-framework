using System;
using Component.Search.Model;

namespace Component.Search.UI.Models;

public class DetailComponentViewModel
{
    public DetailComponent Component { get; set; }
    public Dictionary<string, string> Data { get; set; }
}
