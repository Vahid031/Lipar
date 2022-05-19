using System;

namespace Lipar.Infrastructure.Tools.Utilities.Configurations
{
    public class RabbitMQOptions
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string VirualHost { get; set; }
        public string Protocol { get; set; }
        public string ExchangeName { get; set; }
        public bool ExchangeDurable { get; set; }
        public bool ExchangeAutoDeleted { get; set; }
        public Uri Uri => new Uri($"{Protocol}://{UserName}:{Password}@{Host}:{Port}{VirualHost}");

    }
}
