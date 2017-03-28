using BattleEngine.Actors.Units;
using BattleEngine.Utils;

namespace BattleEngine.Actors.Damages
{
    public class BattleDamageBuildingBullet : BattleDamage
    {
        private int _targetObjectId;
        private double _damage;

        public BattleDamageBuildingBullet()
        {

        }

        public override void update(int tick, int deltaTick)
        {

        }

        public override Vector<ApplyDamageResult> applyDamage(int tick, Vector<ApplyDamageResult> result = null)
        {
            if (result == null) result = new Vector<ApplyDamageResult>();
            var target = engine.context.actors.getActorByObjectId(_targetObjectId) as BattleUnit;
            if (target == null)
            {
                return result;
            }

            target.receiveDamage(_damage);

            var damageResult = new ApplyDamageResult();
            damageResult.units = (int)target.hp;
            damageResult.ownerId = target.ownerId;
            damageResult.targetId = _targetObjectId;
            damageResult.damageObjectId = objectId;
            damageResult.x = transform.x;
            damageResult.y = transform.y;
            damageResult.z = transform.z;

            result.push(damageResult);

            return result;
        }

        public void setDamage(double damage)
        {
            _damage = damage;
        }

        public void setTarget(int objectId)
        {
            _targetObjectId = objectId;
        }

        public override bool needApplyDamage
        {
            get { return true; }
        }

        public override bool needRemove
        {
            get { return true; }
        }

    }
}
