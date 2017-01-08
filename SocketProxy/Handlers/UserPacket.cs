using System;
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

        public object CommandKey => _packet.CommandKey;

        public Type ContentType => _packet.ContentType;

        public object Content => _packet.Content;

        public T ContentAs<T>() where T : class
        {
            return _packet.ContentAs<T>();
        }
    }

    public class UserPacket<T> where T : class
    {
        private readonly UserPacket _userPacket;

        public UserPacket(UserPacket userPacket)
        {
            _userPacket = userPacket;
        }

        public Type ContentType => _userPacket.ContentType;

        public int UserId => _userPacket.UserId;

        public object CommandKey => _userPacket.CommandKey;

        public T Content => _userPacket.ContentAs<T>();
    }
}
