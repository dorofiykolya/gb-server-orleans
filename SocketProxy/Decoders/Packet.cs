using System;

namespace SocketProxy.Decoders
{
    public class Packet
    {
        public PacketType Type;
        public int Id;
        public int RequestId;
        public object CommandKey;

        public byte[] Bytes;
        public object Data;
        public T GetData<T>() where T : class => Data as T;
        public object Content;
        public Type ContentType;
        public T ContentAs<T>() where T : class => Content as T;
    }

    public class Packet<T> where T : class
    {
        public Packet(Packet packet)
        {
            CommandKey = packet.CommandKey;
            Content = packet.ContentAs<T>();
        }

        public object CommandKey { get; }
        public T Content { get; }
    }
}
