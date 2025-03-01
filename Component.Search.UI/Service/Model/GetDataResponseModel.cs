using System;
using Component.Search.Model;

namespace Component.Search.UI.Service.Model;

public class GetDataResponseModel
{
    public SearchTypeModel SearchType { get; set; }
    public List<Dictionary<string, string>> Data { get; set; }
    public int TotalItems { get; set; }
}
