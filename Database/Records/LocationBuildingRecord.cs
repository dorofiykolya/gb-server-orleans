using Newtonsoft.Json;

namespace Database
{
    public class LocationBuildingRecord
    {
        [JsonProperty("position")]
        public int Position;

        [JsonProperty("id")]
        public int Id;

        [JsonProperty("level")]
        public int Level;

        [JsonProperty("coords")]
        public PointRecord Coords;

        [JsonProperty("race")]
        public int Race;
    }
}