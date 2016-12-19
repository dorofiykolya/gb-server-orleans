using Newtonsoft.Json;

namespace SocketProxy.Requests
{
    public class UserAuthStateRequest
    {
        [JsonProperty("userAuthState")]
        public UserAuthStateData UserAuthState;
    }

    public class UserAuthStateData
    {
        [JsonProperty("state")]
        public int State;

        [JsonProperty("minVersion")]
        public string MinVersion;
    }
}
