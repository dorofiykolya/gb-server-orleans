using Newtonsoft.Json;

namespace Records.Units
{
    public class UnitRecord
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("race")]
        public int Race;

        [JsonProperty("speed")]
        public int Speed;

        [JsonProperty("levels")]
        public UnitLevelRecord[] Levels;
    }
}
