using System.Reflection;
using ProxyPackets;
using ProxyPackets.Attributes;

namespace SocketProxy
{
    public class UserPacketConverter : PacketConverter
    {
        public UserPacketConverter()
        {
            var types = typeof(PacketIdAttribute).Assembly.GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes<PacketIdAttribute>();
                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        Add(type, attribute.PacketKey);
                    }
                }
            }
        }
    }
}
