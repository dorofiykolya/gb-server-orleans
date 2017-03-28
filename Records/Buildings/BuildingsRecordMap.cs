using System.Collections.Generic;

namespace Records.Buildings
{
    public class BuildingsRecordMap
    {
        private Dictionary<int, Dictionary<int, BuildingRecord>> _map = new Dictionary<int, Dictionary<int, BuildingRecord>>();

        public BuildingRecord GetByBuildingId(int id, int race)
        {
            Dictionary<int, BuildingRecord> buildings;
            if (_map.TryGetValue(race, out buildings))
            {
                BuildingRecord result;
                buildings.TryGetValue(id, out result);
                return result;
            }
            return null;
        }

        public void Parse(BuildingRecord[] records)
        {
            foreach (var building in records)
            {
                Dictionary<int, BuildingRecord> buildings;
                if (!_map.TryGetValue(building.Race, out buildings))
                {
                    _map[building.Race] = buildings = new Dictionary<int, BuildingRecord>();
                }
                buildings.Add(building.Id, building);
            }
        }
    }
}
