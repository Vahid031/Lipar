using Lipar.Core.Contract.Services;
using System;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;

internal class TestTranslator : ITranslator
{
    public string this[string name] { get => GetString(name); set => throw new NotImplementedException(); }
    public string this[string name, params string[] arguments] { get => GetString(name, arguments); set => throw new NotImplementedException(); }
    public string this[char separator, params string[] names] { get => GetConcateString(separator, names); set => throw new NotImplementedException(); }
    public string GetString(string name)
    {
        return name;
    }
    public string GetString(string pattern, params string[] arguments)
    {
        for (int i = 0; i < arguments.Length; i++)
        {
            arguments[i] = GetString(arguments[i]);
        }
        pattern = GetString(pattern);
        for (int i = 0; i < arguments.Length; i++)
        {
            string placeHolder = $"{{{i}}}";
            pattern = pattern.Replace(placeHolder, arguments[i]);
        }
        return pattern;
    }
    public string GetConcateString(char separator = ' ', params string[] names)
    {
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = GetString(names[i]);
        }
        return string.Join(separator, names);
    }
}

