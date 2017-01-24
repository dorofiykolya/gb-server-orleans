using Newtonsoft.Json;
using Records.Buildings;
using Records.Locations;
using Records.Units;

namespace Records.Initialization
{
    public class InitializationRecord
    {
        [JsonProperty("buildings")]
        public BuildingRecord[] Buildings;

        [JsonProperty("units")]
        public UnitRecord[] Units;

        [JsonProperty("locations")]
        public LocationRecord[] Locations;
    }
}
