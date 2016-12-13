using Newtonsoft.Json;

namespace Database
{
    public class BuildingRecord
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("race")]
        public int Race;

        [JsonProperty("levels")]
        public BuildingLevelRecord[] Levels;

        public BuildingLevelRecord GetLevel(int level)
        {
            return Levels[level];
        }
    }
}