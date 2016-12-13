using System.Collections.Generic;
using System.Linq;

namespace Database
{
    public class DatabaseRecords
    {
        private static DatabaseRecords _instance;

        private readonly InitializationRecord _initializationRecord;
        private readonly Dictionary<long, BuildingRecord> _buildingsMap = new Dictionary<long, BuildingRecord>();
        private readonly Dictionary<long, UnitRecord> _unitsMap = new Dictionary<long, UnitRecord>();
        private readonly Dictionary<int, LocationRecord> _locationaMap = new Dictionary<int, LocationRecord>();

        public DatabaseRecords()
        {
            _initializationRecord = new InitializationRecord();

            var buildingsTableAdapter = new DatabaseDataSetTableAdapters.BuildingsTableAdapter();
            var buildings = buildingsTableAdapter.GetData();

            var buildingLevelAdapter = new DatabaseDataSetTableAdapters.BuildingLevelsTableAdapter();
            var buildingsLevel = buildingLevelAdapter.GetData();

            var unitsTableAdapter = new DatabaseDataSetTableAdapters.UnitsTableAdapter();
            var units = unitsTableAdapter.GetData();

            var unitsLevelTableAdapter = new DatabaseDataSetTableAdapters.UnitLevelsTableAdapter();
            var unitsLevels = unitsLevelTableAdapter.GetData();

            var locationsTableAdapter = new DatabaseDataSetTableAdapters.LocationsTableAdapter();
            var locations = locationsTableAdapter.GetData();

            var locationBuildingsTableAdapter = new DatabaseDataSetTableAdapters.LocationBuildingsTableAdapter();
            var locationsBuildings = locationBuildingsTableAdapter.GetData();

            _initializationRecord.buildings = new BuildingRecord[buildings.Count];

            var index = 0;
            foreach (var building in buildings)
            {
                _initializationRecord.buildings[index] = new BuildingRecord
                {
                    Id = building.Id,
                    Description = building.Description,
                    Name = building.Name,
                    Race = building.Race,
                    Type = building.Type,
                    Levels = buildingsLevel
                    .Where(b => b.Id == building.Id && b.Race == building.Race)
                    .OrderBy(b => b.Level)
                    .Select(b => new BuildingLevelRecord
                    {
                        UnitId = b.UnitsId,
                        Units = b.Units,
                        AttackRange = b.AttackRange,
                        AttackSpeed = b.AttackSpeed,
                        Damage = b.Damage,
                        Icon = b.Icon,
                        MannaProduction = b.MannaProduction,
                        UnitsMaxProduction = b.UnitsMaxProduction,
                        UnitsProduction = b.UnitsProduction,
                        View = b.View
                    }).ToArray()
                };
                index++;
            }

            _initializationRecord.units = new UnitRecord[units.Count];
            index = 0;
            foreach (var unit in units)
            {
                _initializationRecord.units[index] = new UnitRecord
                {
                    Id = unit.Id,
                    Race = unit.Race,
                    Name = unit.Name,
                    Description = unit.Description,
                    Speed = unit.Speed,
                    Levels = unitsLevels
                    .Where(u => u.Id == unit.Id && u.Race == unit.Race)
                    .OrderBy(u => u.Level)
                    .Select(u => new UnitLevelRecord
                    {
                        Damage = u.Damage,
                        Icon = u.Icon,
                        View = u.View,
                        Defense = u.Defense,
                        Hp = u.Hp,
                        MagicDefense = u.MagicDefense
                    }).ToArray()
                };
                index++;
            }

            _initializationRecord.locations = new LocationRecord[locations.Count];
            index = 0;
            foreach (var location in locations)
            {
                _initializationRecord.locations[index] = new LocationRecord
                {
                    Id = location.Id,
                    Name = location.Name,
                    Decsription = location.Description,
                    Icon = location.Icon,
                    View = location.View,
                    Buildings = locationsBuildings
                    .Where(l => l.LocationId == location.Id)
                    .Select(l => new LocationBuildingRecord
                    {
                        Id = l.BuildingId,
                        Level = l.BuildingLevel,
                        Coords = new PointRecord
                        {
                            X = l.X,
                            Y = l.Y
                        },
                        Race = l.Race,
                        Position = l.Position
                    }).ToArray()
                };
                index++;
            }

            foreach (var building in _initializationRecord.buildings)
            {
                _buildingsMap[GetIndex(building.Id, building.Race)] = building;
            }

            foreach (var unit in _initializationRecord.units)
            {
                _unitsMap[GetIndex(unit.Id, unit.Race)] = unit;
            }

            foreach (var location in _initializationRecord.locations)
            {
                _locationaMap[location.Id] = location;
            }
        }

        public static DatabaseRecords Instance => _instance ?? (_instance = new DatabaseRecords());

        public InitializationRecord Initialization => Instance._initializationRecord;

        public UnitRecord GetUnitRecord(int unitId, int race)
        {
            UnitRecord result;
            _unitsMap.TryGetValue(GetIndex(unitId, race), out result);
            return result;
        }

        public BuildingRecord GetBuildingRecord(int buildingId, int race)
        {
            BuildingRecord result;
            _buildingsMap.TryGetValue(GetIndex(buildingId, race), out result);
            return result;
        }

        public LocationRecord GetLocationRecord(int locationId)
        {
            LocationRecord result;
            _locationaMap.TryGetValue(locationId, out result);
            return result;
        }

        private static long GetIndex(int id, int race)
        {
            return ((long)id << 32) | (long)race;
        }
    }
}
