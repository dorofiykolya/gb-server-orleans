using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleActors
    {
        private Vector<BattleObject> _temp = new Vector<BattleObject>();
		private _tempActors:Vector.<BattleActorsGroup> = new Vector.<BattleActorsGroup>();
		private _tempComponents:Vector.<Component> = new Vector.<Component>();
		
		private _battleEngine:BattleEngine;
		private _groupMap:Dictionary;
		private _factory:BattleObjectFactory;
		private _list:Vector.<BattleActorsGroup>;
		private _map:Vector.<BattleObject>;
		private _damages:BattleDamages;
		
		public function BattleActors(battleEngine:BattleEngine)
        {
            _battleEngine = battleEngine;
            _groupMap = new Dictionary();
            _map = new < BattleObject >[null];
            _factory = new BattleObjectFactory(_map, _battleEngine);
            _list = new Vector.< BattleActorsGroup > ();
            _damages = new BattleDamages(_battleEngine.context);

            for each(var enum: ActorsGroup in Enum.getEnums(ActorsGroup))
			{

                group(enum);
			}
}

public function get damagesFactory():BattleDamages
		{
			return _damages;
		}
		
		public function get factory():BattleObjectFactory
		{
			return _factory;
		}
		
		public function getGroups(result:Vector.<BattleActorsGroup> = null):Vector.<BattleActorsGroup>
		{
			if (result == null) result = new Vector.<BattleActorsGroup>();
			for each(var item:BattleActorsGroup in _battleEngine.getComponents(BattleActorsGroup, false, _tempComponents))
			{
				result.push(item);
			}
			return result;
		}
		
		public function getActors(result:Vector.<BattleObject> = null):Vector.<BattleObject>
		{
			for each(var item:BattleActorsGroup in _list)
{
    item.getActors(null, result);
}
			return result;
		}
		
		public function getActorByObjectId(objectId:int):BattleObject
		{
			return _map[objectId];
		}
		
		public function group(enum: ActorsGroup):BattleActorsGroup
		{
			var result:BattleActorsGroup = _groupMap[enum];
			if (result == null)
			{
				_groupMap[enum] = result = new BattleActorsGroup(enum, _battleEngine);
				_list.push(result);
				_battleEngine.addComponent(result);
			}
			return result;
		}
		
		public function get buildings():BattleActorsGroup
		{
			return group(ActorsGroup.BUILDING);
		}
		
		public function get units():BattleActorsGroup
		{
			return group(ActorsGroup.UNIT);
		}
		
		public function get bullets():BattleActorsGroup
		{
			return group(ActorsGroup.BULLET);
		}
		
		public function get damages():BattleActorsGroup
		{
			return group(ActorsGroup.DAMAGE);
		}
    }
}
