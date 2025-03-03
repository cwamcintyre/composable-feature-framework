using System;

namespace Component.Form.Model;

public class FormData
{
    public Dictionary<string, string> Data { get; set; }
    public Stack<string> Route { get; set; }
}
