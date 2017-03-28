using BattleEngine.Actors.Buildings;
using BattleEngine.Actors.Damages;
using BattleEngine.Actors.Units;
using BattleEngine.Output;

namespace BattleEngine.Actors.Bullets
{
    public class BattleBulletBuilding : BattleBullet
    {
        private BattleUnit _target;
        private BulletMoveComponent _move;
        private double _damage;

        public BattleBulletBuilding()
        {
            _move = (AddComponent<BulletMoveComponent>());
        }

        override public void update(int tick, int deltaTick)
        {
            var move = GetComponent<BulletMoveComponent>() as BulletMoveComponent;
            if (move.updatePosition(deltaTick))
            {
                var evt = engine.output.enqueueByFactory<BulletMoveEvent>() as BulletMoveEvent;
                evt.objectId = this.objectId;

                evt.tick = tick;
                evt.x = this.transform.x;
                evt.y = this.transform.y;
                evt.z = this.transform.z;
            }
        }

        override public BattleDamage generateDamage()
        {
            var damage = engine.context.actors.factory.instantiate<BattleDamageBuildingBullet>() as BattleDamageBuildingBullet;
            damage.transform.setFrom(transform);
            damage.setOwnerId(ownerId);
            damage.setDamage(_damage);
            damage.setTarget(_target.objectId);
            return damage;
        }

        public void setInfoFrom(BattleBuilding building)
        {
            var damage = building.infoLevel.Damage;
            transform.setFrom(building.transform);
            _damage = building.engine.players.getPlayer(building.ownerId).modifier.calculate(Modifiers.ModifierType.BUILDING_DAMAGE, damage, building.info.Id);
        }

        override public bool needGenerateDamage
        {
            get { return _move.moveCompleted; }
        }

        override public bool needRemove
        {
            get { return _move.moveCompleted; }
        }

        public BattleUnit target
        {
            get { return _target; }
            set
            {
                _target = value;
                _move.moveTo(value.transform);
            }
        }

    }
}
