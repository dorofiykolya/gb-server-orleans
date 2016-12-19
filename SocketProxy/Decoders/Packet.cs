namespace SocketProxy.Decoders
{
    public class Packet
    {
        public byte[] Bytes;
        public object Data { get; set; }
        public T GetData<T>() where T : class => Data as T;
        public object Command { get; set; }
        public object Content;
        public T ContentAs<T>() where T : class => Content as T;
    }

    public class Packet<T> where T : class
    {
        public Packet(Packet packet)
        {
            Command = packet.Command;
            Content = packet.ContentAs<T>();
        }

        public object Command { get; }
        public T Content { get; }
    }
}
