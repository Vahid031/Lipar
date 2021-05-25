﻿using System;

namespace Lipar.Infrastructure.Tools.Utilities.Services
{
    public interface IJson
    {
        string SerializeObject<TInput>(TInput input);
        TOutput DeserializeObject<TOutput>(string input);
        object DeserializeObject(string input, Type type);
    }
}