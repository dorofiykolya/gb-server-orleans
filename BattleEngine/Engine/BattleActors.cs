using System;
using System.Collections.Generic;
using BattleEngine.Actors;
using BattleEngine.Utils;
using Common.Composite;

namespace BattleEngine.Engine
{
    public class BattleActors
    {
        private Vector<BattleObject> _temp = new Vector<BattleObject>();
        private Vector<BattleActorsGroup> _tempActors = new Vector<BattleActorsGroup>();
        private Vector<Component> _tempComponents = new Vector<Component>();

        private BattleEngine _battleEngine;
        private Dictionary<ActorsGroup, BattleActorsGroup> _groupMap;
        private BattleObjectFactory _factory;
        private Vector<BattleActorsGroup> _list;
        private Vector<BattleObject> _map;
        private BattleDamages _damages;

        public BattleActors(BattleEngine battleEngine)
        {
            _battleEngine = battleEngine;
            _groupMap = new Dictionary<ActorsGroup, BattleActorsGroup>();
            _map = new Vector<BattleObject>();
            _factory = new BattleObjectFactory(_map, _battleEngine);
            _list = new Vector<BattleActorsGroup>();
            _damages = new BattleDamages(_battleEngine.context);

            foreach (var e in Enum.GetValues(typeof(ActorsGroup)))
            {
                group((ActorsGroup)e);
            }
        }

        public BattleDamages damagesFactory
        {
            get { return _damages; }
        }

        public BattleObjectFactory factory
        {
            get { return _factory; }
        }

        public Vector<BattleActorsGroup> getGroups(Vector<BattleActorsGroup> result = null)
        {
            if (result == null) result = new Vector<BattleActorsGroup>();
            foreach (var item in _battleEngine.GetComponents(typeof(BattleActorsGroup), false, _tempComponents))
            {
                result.push((BattleActorsGroup)item);
            }
            return result;
        }

        public Vector<BattleObject> getActors(Vector<BattleObject> result = null)
        {
            foreach (var item in _list)
            {
                item.getActors(null, result);
            }
            return result;
        }

        public BattleObject getActorByObjectId(int objectId)
        {
            return _map[objectId];
        }

        public BattleActorsGroup group(ActorsGroup e)
        {
            BattleActorsGroup result;
            if (!_groupMap.TryGetValue(e, out result))
            {
                _groupMap[e] = result = new BattleActorsGroup(e, _battleEngine);
                _list.push(result);
                _battleEngine.AddComponent(result);
            }
            return result;
        }

        public BattleActorsGroup buildings
        {
            get { return group(ActorsGroup.BUILDING); }
        }

        public BattleActorsGroup units
        {
            get { return group(ActorsGroup.UNIT); }
        }

        public BattleActorsGroup bullets
        {
            get { return group(ActorsGroup.BULLET); }
        }

        public BattleActorsGroup damages
        {
            get { return group(ActorsGroup.DAMAGE); }
        }
    }
}
