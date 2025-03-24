using System;

namespace Component.Form.UI.Exceptions;

public class BadRouteParametersException : Exception
{
    public BadRouteParametersException(string message) : base(message)
    {
    }

    public BadRouteParametersException(string message, Exception innerException) : base(message, innerException)
    {
    }
}