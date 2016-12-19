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

        public Task<object> ConvertAsync(string command, object value)
        {
            Type type;
            if (_map.TryGetValue(command, out type))
            {
                return Task.Run(() => ((JObject) value).ToObject(type));
            }
            return Task.FromResult(value);
        }
    }
}
