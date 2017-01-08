using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SocketProxy.Packets
{
    public class UserMessagePacket
    {
        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("message")]
        public string Message;
    }
}
