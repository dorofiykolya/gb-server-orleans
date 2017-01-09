using System;
using Newtonsoft.Json;
using ProxyPackets.Attributes;

namespace ProxyPackets
{
    [Serializable]
    [PacketId("authByDeveloper")]
    public class AuthByDeveloperPacket
    {
        [JsonProperty("developerId")]
        public string DeveloperId;
    }
}
