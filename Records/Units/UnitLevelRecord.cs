using Newtonsoft.Json;

namespace Records.Units
{
    public class UnitLevelRecord
    {
        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("view")]
        public string View;

        [JsonProperty("hp")]
        public int Hp;

        [JsonProperty("damage")]
        public int Damage;

        [JsonProperty("defense")]
        public int Defense;

        [JsonProperty("magicDefense")]
        public int MagicDefense;
    }
}
