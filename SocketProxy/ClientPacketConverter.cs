using SocketProxy.Packets;

namespace SocketProxy
{
    public class ClientPacketConverter : PacketConverter
    {
        public ClientPacketConverter()
        {
            Add<AuthByDeveloperPacket>("authByDeveloper");
            Add<UserAuthPacket>("userAuth");
        }
    }
}
