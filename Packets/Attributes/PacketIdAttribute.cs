using System;

namespace ProxyPackets.Attributes
{
    public class PacketIdAttribute : Attribute
    {
        public PacketIdAttribute(string packetKey)
        {
            PacketKey = packetKey;
        }

        public string PacketKey { get; }
    }
}
