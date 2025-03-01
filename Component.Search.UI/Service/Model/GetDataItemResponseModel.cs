using System;
using Component.Search.Model;

namespace Component.Search.UI.Service.Model;

public class GetDataItemResponseModel
{
    public DetailTypeModel DetailTypeModel { get; set; }
    public Dictionary<string, string> Data { get; set; }
}
