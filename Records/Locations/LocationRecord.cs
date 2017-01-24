using Newtonsoft.Json;

namespace Records.Locations
{
    public class LocationRecord
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("decsription")]
        public string Decsription;

        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("view")]
        public string View;

        [JsonProperty("buildings")]
        public LocationBuildingRecord[] Buildings;
    }
}
