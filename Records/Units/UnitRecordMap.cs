using System;
using System.Collections.Generic;

namespace Records.Units
{
    public class UnitRecordMap
    {
        private Dictionary<int, Dictionary<int, UnitRecord>> _map = new Dictionary<int, Dictionary<int, UnitRecord>>();

        public UnitRecord GetBy(int unitId, int race)
        {
            Dictionary<int, UnitRecord> units;
            if (_map.TryGetValue(race, out units))
            {
                UnitRecord result;
                units.TryGetValue(unitId, out result);
                return result;
            }
            return null;
        }

        public void Parse(UnitRecord[] records)
        {
            _map.Clear();
            foreach (var unit in records)
            {
                Dictionary<int, UnitRecord> units;
                if (!_map.TryGetValue(unit.Race, out units))
                {
                    _map[unit.Race] = units = new Dictionary<int, UnitRecord>();
                }
                units.Add(unit.Id, unit);
            }
        }
    }
}
