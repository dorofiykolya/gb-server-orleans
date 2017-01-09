using System;
using Newtonsoft.Json;
using ProxyPackets.Attributes;

namespace ProxyPackets
{
    [Serializable]
    [PacketId("userAuth")]
    public class UserAuthPacket
    {
        [JsonProperty("userKey")]
        public int UserKey;

        [JsonProperty("authTS")]
        public int AuthTs;

        [JsonProperty("authKey")]
        public string AuthKey;

        [JsonProperty("isBrowser")]
        public bool IsBrowser;
    }
}
