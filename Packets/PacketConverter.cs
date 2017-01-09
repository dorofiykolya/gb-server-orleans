using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ProxyPackets
{
    public class PacketConverter
    {
        private readonly Dictionary<string, Type> _map = new Dictionary<string, Type>();

        protected void Add<T>(string command)
        {
            Add(typeof(T), command);
        }

        protected void Add(Type type, string command)
        {
            _map[command] = type;
        }

        public Task<object> ConvertAsync(string command, object value, out Type type)
        {
            if (_map.TryGetValue(command, out type))
            {
                Type objectType = type;
                return Task.Run(() =>
                {
                    return ((JObject) value).ToObject(objectType);
                });
            }
            return Task.FromResult(value);
        }
    }
}
