using Newtonsoft.Json;

namespace Database
{
    public class BuildingLevelRecord
    {
        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("view")]
        public string View;

        [JsonProperty("unitId")]
        public int UnitId;

        [JsonProperty("units")]
        public int Units;

        [JsonProperty("unitsProduction")]
        public int UnitsProduction;

        [JsonProperty("unitsMaxProduction")]
        public int UnitsMaxProduction;

        [JsonProperty("attackSpeed")]
        public int AttackSpeed;

        [JsonProperty("attackRange")]
        public int AttackRange;

        [JsonProperty("damage")]
        public int Damage;

        [JsonProperty("mannaProduction")]
        public int MannaProduction;
    }
}