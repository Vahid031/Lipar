using System;

namespace Lipar.Core.Contract.Services;

public interface IJsonService
{
    string SerializeObject<TInput>(TInput input);
    TOutput DeserializeObject<TOutput>(string input);
    object DeserializeObject(string input, Type type);
}


