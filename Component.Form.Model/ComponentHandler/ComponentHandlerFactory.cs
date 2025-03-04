using System;

namespace Component.Form.Model.ComponentHandler;

public class ComponentHandlerFactory
{
    private readonly IEnumerable<IComponentHandler> _parsers;

    public ComponentHandlerFactory(IEnumerable<IComponentHandler> parsers)
    {
        _parsers = parsers;
    }

    public IComponentHandler GetFor(string type) 
    {
        foreach (var parser in _parsers)
        {
            if (parser.IsFor(type))
            {
                return parser;
            }
        }

        // no parser found, assume SimpleHandler
        return SimpleHandler.Instance;
    }

    public HashSet<string> GetAllTypes()
    {
        return _parsers.Select(p => p.GetDataType()).ToHashSet();
    }

    public static string GetDataType(Type type)
    {
        return $"{type.Name} {type.Assembly.GetName().Name}";
    }
}
