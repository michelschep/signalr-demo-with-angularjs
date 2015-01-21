using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoSignalR
{
    public class Envelope<T>
    {
        public readonly IDictionary<string, string> Meta;
        public readonly T Payload;

        public Envelope(T payload) : this(payload, Enumerable.Empty<KeyValuePair<string, string>>())
        {
        }

        Envelope(T payload, IEnumerable<KeyValuePair<string, string>> meta)
        {
            Payload = payload;
            Meta = new Dictionary<string, string>(meta.ToDictionary(p => p.Key, p => p.Value));
        }

        public Envelope<T> WithMeta(string key, string value)
        {
            return new Envelope<T>(Payload, new Dictionary<string, string>(Meta){{key, value}});
        }

        public override string ToString()
        {
            return String.Format("Envelope<{0}>[{1}]", typeof (T).Name, Payload);
        }
    }
}