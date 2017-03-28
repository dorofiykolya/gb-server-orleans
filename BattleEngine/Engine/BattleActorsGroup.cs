using System;
using BattleEngine.Actors;
using BattleEngine.Utils;
using Common.Composite;

namespace BattleEngine.Engine
{
    public class BattleActorsGroup : Entity
    {
        private Vector<Component> _temp = new Vector<Component>();

        private ActorsGroup _group;
        private BattleEngine _battleEngine;

        public BattleActorsGroup(ActorsGroup group, BattleEngine battleEngine)
        {
            _battleEngine = battleEngine;
            _group = group;
        }

        public Vector<BattleObject> getActors(Type type = null, Vector<BattleObject> result = null)
        {
            if (result == null) result = new Vector<BattleObject>();
            _temp.length = 0;
            foreach (var item in GetComponents(type, false, _temp))
            {
                if (type == null || item.GetType().IsAssignableFrom(type))
                {
                    result.push((BattleObject)item);
                }
            }
            return result;
        }

        public Vector<BattleObject> getActorsInRange(double x, double y, double range, Type type = null, Vector<BattleObject> result = null)
        {
            if (result == null) result = new Vector<BattleObject>();
            _temp.length = 0;
            foreach (BattleObject item in GetComponents(type, false, _temp))
            {
                if (type == null || item.GetType().IsAssignableFrom(type))
                {
                    if (item.transform.positionDistanceTo(x, y) <= range)
                    {
                        result.push(item);
                    }
                }
            }
            return result;
        }

        public ActorsGroup group()
        {
            return _group;
        }
    }
}
