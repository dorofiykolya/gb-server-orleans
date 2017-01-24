using Newtonsoft.Json;

namespace Records.Buildings
{
    public class BuildingRecord
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("buildingId")]
        public int BuildingId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("type")]
        public BuildingType Type;

        [JsonProperty("race")]
        public int Race;

        [JsonProperty("levels")]
        public BuildingLevelRecord[] Levels;
    }
}
