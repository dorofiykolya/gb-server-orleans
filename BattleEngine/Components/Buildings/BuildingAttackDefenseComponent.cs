using BattleEngine.Actors.Buildings;
using BattleEngine.Actors.Bullets;
using BattleEngine.Actors.Units;
using BattleEngine.Engine;
using BattleEngine.Output;
using System;

namespace BattleEngine.Components.Buildings
{
    public class BuildingAttackDefenseComponent : BattleComponent
    {
        private int _remainingTicksToNextAttack;

        public BuildingAttackDefenseComponent()
        {

        }

        public int ticksBetweenAttack
        {
            get
            {
                var buildng = (BattleBuilding)target;
                var result = engine.players.getPlayer(target.ownerId).modifier.calculate(Modifiers.ModifierType.BUILDING_ATTACK_RANGE, buildng.infoLevel.AttackSpeed, buildng.info.Id);
                return (int)result;
            }
        }

        public void attack(BattleUnit targetUnit)
        {
            _remainingTicksToNextAttack = ticksBetweenAttack;

            var bullet = engine.context.actors.factory.bulletFactory.instantiate<BattleBulletBuilding>() as BattleBulletBuilding;
            bullet.target = targetUnit;
            bullet.setInfoFrom((BattleBuilding)(target));
            bullet.setOwnerId(this.target.ownerId);
            engine.context.actors.bullets.AddComponent(bullet);

            var evt = engine.output.enqueueByFactory<BulletCreateEvent>() as BulletCreateEvent;
            evt.tick = engine.tick;
            evt.fromObjectId = target.objectId;
            evt.toObjectId = targetUnit.objectId;
            evt.objectId = bullet.objectId;
        }

        public double range
        {
            get { return ((BattleBuilding)(target)).infoLevel.AttackRange; }
        }

        public bool canAttack
        {
            get { return _remainingTicksToNextAttack <= 0; }
        }

        public double remainingTicksToNextAttack
        {
            get { return _remainingTicksToNextAttack; }
        }

        override public bool needRemove
        {
            get { return false; }
        }

        override public void update(int tick, int deltaTick)
        {
            base.update(tick, deltaTick);
            _remainingTicksToNextAttack = Math.Max(0, _remainingTicksToNextAttack - deltaTick);
        }

    }
}
