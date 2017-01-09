using System;
using Newtonsoft.Json;
using ProxyPackets.Attributes;

namespace ProxyPackets
{
    [Serializable]
    [PacketId("message")]
    public class UserMessagePacket
    {
        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("message")]
        public string Message;
    }
}
