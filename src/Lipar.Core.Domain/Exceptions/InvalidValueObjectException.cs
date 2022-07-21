using System;

namespace Lipar.Core.Domain.Exceptions;

public class InvalidValueObjectException : Exception
{
    public string[] Parameters { get; set; }
    public InvalidValueObjectException(string message, params string[] parameters)
    : base(message)
    {
        Parameters = parameters;
    }
}

