using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleEngine.Actors;
using BattleEngine.Actors.Buildings;
using BattleEngine.Engine;
using BattleEngine.Modifiers;
using BattleEngine.Players;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleMannaModule : BattleModule
    {

        private Vector<BattleObject> _temp = new Vector<BattleObject>();
        private Dictionary<int, double> _lastManna = new Dictionary<int, double>();

        public BattleMannaModule()
        {
        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            _temp.length = 0;
            context.actors.buildings.getActors(typeof(BattleBuilding), _temp);

            saveLastManna(context, tick);

            foreach (var item in _temp)
            {
                var mannaComponent = item.GetComponent<MannaRegenComponent>();
                if (mannaComponent != null)
                {
                    var increaseManna = mannaComponent.mannaPerTick * deltaTick;

                    if (increaseManna != 0)
                    {
                        var player = context.players.getPlayer(item.ownerId);
                        var result = player.modifier.calculate(ModifierType.MANNA_INCREASE, increaseManna);
                        player.manna.add(result);
                    }
                }
            }
            checkManna(context, tick);
        }

        private void saveLastManna(BattleContext context, int tick)
        {
            foreach (var battlePlayer in context.players.players)
            {
                _lastManna[battlePlayer.id] = battlePlayer.manna.value;
            }
        }

        private void checkManna(BattleContext context, int tick)
        {
            foreach (var player in context.players.players)
            {
                if (_lastManna[player.id] != player.manna.value)
                {
                    var evt = context.output.enqueueByFactory<MannaChangeEvent>();
                    evt.ownerId = player.id;
                    evt.manna = player.manna.value;
                    evt.tick = tick;
                }
            }
        }
    }
}
