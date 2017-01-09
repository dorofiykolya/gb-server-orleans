using System.Reflection;

namespace SocketProxy.Users
{
    public class UserCommandFactoryProvider : CommandFactoryProvider
    {
        public UserCommandFactoryProvider()
        {
            var types = typeof(PacketCommandAttribute).Assembly.GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes<PacketCommandAttribute>();
                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        Add(attribute.Packet, type);
                    }
                }
            }
        }
    }
}
