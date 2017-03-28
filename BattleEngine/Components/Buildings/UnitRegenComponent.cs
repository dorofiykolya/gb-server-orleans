using BattleEngine.Actors.Buildings;
using BattleEngine.Engine;

namespace BattleEngine.Components.Buildings
{
    public class UnitRegenComponent : BattleEngine.Engine.BattleComponent
    {
        private double _unitsPerTick;
        private double _increase;

        public UnitRegenComponent()
        {
            _increase = 0;
        }

        override protected void OnAttach()
        {
            base.OnAttach();

            var unitsPerSecond = ((BattleBuilding)target).unitsPerSecond;
            _unitsPerTick = BattleUtils.floor(unitsPerSecond / engine.configuration.ticksPerSecond);
        }

        public BattleBuilding building
        {
            get { return (BattleBuilding)target; }
        }

        override public bool needRemove
        {
            get { return false; }
        }

        public int units
        {
            get { return building.units.count; }
        }

        public double unitsPerTick
        {
            get { return _unitsPerTick; }
        }

        public void setUnitsPerTick(double value)
        {
            _unitsPerTick = value;
        }

        public int removeHalf()
        {
            var result = units / 2;
            building.units.remove(result);
            return result;
        }

        public bool regen(double deltaTick)
        {
            if (building.units.count >= building.infoLevel.UnitsMaxProduction)
            {
                return false;
            }

            var increaseValue = unitsPerTick * deltaTick;
            increaseValue = engine.players.getPlayer(building.ownerId).modifier.calculate(Modifiers.ModifierType.UNITS_INCREASE, increaseValue);
            _increase += increaseValue;

            var newUnits = (int)(_increase);
            if (newUnits > 0)
            {
                _increase -= newUnits;
            }

            var lastUnits = units;
            if (lastUnits + newUnits > building.infoLevel.UnitsMaxProduction)
            {
                newUnits = building.infoLevel.UnitsMaxProduction - building.units.count;
            }
            building.units.add(newUnits);
            return lastUnits != units;
        }
    }
}
