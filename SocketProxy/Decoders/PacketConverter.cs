using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SocketProxy
{
    public class PacketConverter
    {
        private readonly Dictionary<string, Type> _map = new Dictionary<string, Type>();

        protected void Add<T>(string command)
        {
            _map[command] = typeof(T);
        }

        public Task<object> ConvertAsync(string command, object value, out Type type)
        {
            if (_map.TryGetValue(command, out type))
            {
                Type objectType = type;
                return Task.Run(() => ((JObject) value).ToObject(objectType));
            }
            return Task.FromResult(value);
        }
    }
}
