using Newtonsoft.Json;

namespace Database
{
    public class PointRecord
    {
        [JsonProperty("x")]
        public int X;

        [JsonProperty("y")]
        public int Y;
    }
}
