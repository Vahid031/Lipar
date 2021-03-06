using System;

namespace Lipar.Core.Domain.Exceptions;

public class InvalidEntityStateException : Exception
{
    public string[] Parameters { get; set; }
    public InvalidEntityStateException(string message, params string[] parameters)
    : base(message)
    {
        Parameters = parameters;
    }
}

