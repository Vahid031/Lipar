using System;

namespace Lipar.Core.Domain.Events
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class EventTopicAttribute : Attribute
    {
        public string Topic { get; set; }

        private EventTopicAttribute() { }
        public EventTopicAttribute(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentNullException();

            Topic = topic;
        }
    }
}
