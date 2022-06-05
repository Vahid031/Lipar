using System;
using System.Globalization;
using Lipar.Core.Contract.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.AspNetCore.Http;

namespace Zamin.Infra.Tools.Localizer.Parrot
{
    public class ParrotTranslator : ITranslator
    {
        private readonly ParrotDataWrapper _localizer;
        private readonly string _currentCulture;
        public ParrotTranslator(LiparOptions liparOptions, IHttpContextAccessor httpContextAccessor)
        {

            string accetpLanguage = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Trim();
            if (string.IsNullOrEmpty(accetpLanguage) || accetpLanguage == "*" || accetpLanguage.Length < 5)
                _currentCulture = CultureInfo.CurrentCulture.ToString();
            else
                _currentCulture = accetpLanguage.Substring(0, 5);

            _localizer = ParrotDataWrapper.CreateFactory(liparOptions);
        }

        public string this[string name] { get => GetString(name); set => throw new NotImplementedException(); }
        public string this[string name, params string[] arguments] { get => GetString(name, arguments); set => throw new NotImplementedException(); }
        public string this[char separator, params string[] names] { get => GetConcateString(separator, names); set => throw new NotImplementedException(); }
        public string GetString(string name)
        {
            return _localizer.Get(name, _currentCulture);
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
}
