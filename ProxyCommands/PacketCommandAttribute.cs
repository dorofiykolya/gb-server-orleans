using System;

namespace SocketProxy.Users
{
    public class PacketCommandAttribute : Attribute
    {
        public PacketCommandAttribute(Type packet)
        {
            Packet = packet;
        }

        public Type Packet { get; }
    }
}
