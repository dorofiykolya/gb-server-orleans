using SocketProxy.Decoders;

namespace SocketProxy.Handlers
{
    public class UserPacket
    {
        private readonly Packet _packet;
        private readonly Auth _auth;

        public UserPacket(Packet packet, Auth auth)
        {
            _packet = packet;
            _auth = auth;
        }

        public int UserId => _auth.UserId;

        public object Command => _packet.Command;

        public T ContentAs<T>() where T : class
        {
            return _packet.ContentAs<T>();
        }
    }

    public class UserPacket<T> : UserPacket where T : class
    {
        public UserPacket(Packet packet, Auth auth) : base(packet, auth)
        {
        }

        public T Content => base.ContentAs<T>();
    }
}
