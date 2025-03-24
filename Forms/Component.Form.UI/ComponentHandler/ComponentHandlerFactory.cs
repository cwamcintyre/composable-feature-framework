namespace Component.Form.UI.ComponentHandler;

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
        
        throw new NoComponentHandlerFoundException($"No component handler found for type {type}");
    }
}

public class NoComponentHandlerFoundException : Exception
{
    public NoComponentHandlerFoundException(string message) : base(message)
    {
    }
}
