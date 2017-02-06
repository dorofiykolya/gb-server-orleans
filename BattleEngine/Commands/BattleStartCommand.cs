using BattleEngine.Actions;
using BattleEngine.Actors.Buildings;
using BattleEngine.Engine;

namespace BattleEngine.Commands
{
    public class BattleStartCommand : BattleEngineCommand
    {

        public BattleStartCommand() : base(typeof(BattleStartAction))
        {

        }

        public override void execute(BattleAction action, BattleContext context)
        {
            addBuildings(action, context);
        }

        private void addBuildings(BattleAction action, BattleContext context)
        {
            foreach (var record in context.configuration.buildings)
            {
                var battleObject = context.actors.factory.buildingFactory.instantiate<BattleBuilding>();
                context.actors.buildings.AddComponent(battleObject);
                battleObject.initialize(record, context.configuration);

                var evt = context.output.enqueueByFactory<BuildingCreateEvent>();
                evt.objectId = battleObject.objectId;
                evt.buildingId = battleObject.info.id;
                evt.level = battleObject.level;
                evt.ownerId = battleObject.ownerId;
                evt.tick = action.tick;
                evt.x = battleObject.transform.x;
                evt.y = battleObject.transform.y;
                evt.units = battleObject.units.count;
            }
        }
    }
}
