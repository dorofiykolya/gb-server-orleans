namespace SocketProxy.Decoders
{
    public class Packet
    {
        public object Data { get; set; }
        public T GetData<T>() where T : class => Data as T;
    }
}
