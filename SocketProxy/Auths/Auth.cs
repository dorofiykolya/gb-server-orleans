using System;
using Newtonsoft.Json;

namespace SocketProxy
{
    public class Auth
    {
        [JsonProperty("userKey")]
        public int UserId;
        [JsonProperty("authKey")]
        public string AuthKey;
        [JsonProperty("authTime")]
        public DateTime AuthTime;
    }
}
