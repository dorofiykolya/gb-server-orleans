using System;
using Newtonsoft.Json;

namespace SocketProxy
{
    public class Auth
    {
        [JsonProperty("userId")]
        public int UserId;
        [JsonProperty("authKey")]
        public string AuthKey;
        [JsonProperty("authTime")]
        public DateTime AuthTime;
        [JsonProperty("isNewAppUser")]
        public bool IsNewAppUser;
    }
}
