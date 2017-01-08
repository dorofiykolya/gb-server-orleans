using SocketProxy.Packets;

namespace SocketProxy
{
    public class UserPacketConverter : PacketConverter
    {
        public UserPacketConverter()
        {
            Add<AuthByDeveloperPacket>("authByDeveloper");
            Add<UserAuthPacket>("userAuth");
            Add<InitUserPacket>("initUser");
        }
    }
}
