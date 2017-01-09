using System;
using Newtonsoft.Json;
using ProxyPackets.Attributes;

namespace ProxyPackets
{
    [Serializable]
    [PacketId("userAuth")]
    public class UserAuthPacket
    {
        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("authTS")]
        public int AuthTs;

        [JsonProperty("authKey")]
        public string AuthKey;

        [JsonProperty("isBrowser")]
        public bool IsBrowser;
    }
}
