using Newtonsoft.Json;
using ProxyPackets.Attributes;

namespace ProxyPackets
{
    [PacketId("changeUserName")]
    public class ChangeUserNamePacket
    {
        [JsonProperty("name")]
        public string Name;
    }
}
