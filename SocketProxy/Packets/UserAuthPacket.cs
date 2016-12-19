using Newtonsoft.Json;

namespace SocketProxy.Packets
{
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
