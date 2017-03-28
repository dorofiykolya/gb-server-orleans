using BattleEngine.Actors.Buildings;
using BattleEngine.Engine;
using BattleEngine.Utils;
using Common.Composite;

namespace BattleEngine.Components.Buildings
{
    public class MannaRegenComponent : BattleComponent
    {
        private Vector<Component> _temp = new Vector<Component>();
        private double _mannaPerTick;

        public MannaRegenComponent()
        {
            _mannaPerTick = 1;
        }

        override protected void OnAttach()
        {
            base.OnAttach();
            _mannaPerTick = ((BattleBuilding)(target)).mannaPerSecond / engine.configuration.ticksPerSecond;
        }

        override public bool needRemove
        {
            get { return false; }
        }

        public double mannaPerTick
        {
            get
            {
                var currentMannaPerTick = _mannaPerTick;

                _temp.length = 0;
                GetComponents(typeof(IMannaModifier), false, _temp);
                foreach (IMannaModifier item in _temp)
                {
                    currentMannaPerTick *= item.mannaModifierPercent;
                }

                return currentMannaPerTick;
            }
        }

    }
}
