using Newtonsoft.Json;

namespace SocketProxy.Packets
{
    public class AuthByDeveloperPacket
    {
        [JsonProperty("developerId")]
        public string DeveloperId;
    }
}
