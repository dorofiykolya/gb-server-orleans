using Newtonsoft.Json;

namespace Records.Locations
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
    }
}
