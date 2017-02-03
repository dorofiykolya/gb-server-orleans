using BattleEngine.Modifiers;
using BattleEngine.Records;
using Common.Composite;

namespace BattleEngine.Players
{
    public class BattlePlayer : Entity
    {
        private int _id;
        private PlayerManna _manna;
        private PlayerModifier _modifier;
        private int _race;
        private int _index;

        public BattlePlayer()
        {
            _manna = AddComponent<PlayerManna>();
            _modifier = AddComponent<PlayerModifier>();
        }

        public void initialize(BattleOwnerRecord item, int index)
        {
            _index = index;
            _id = item.id;
            _race = item.race;
        }

        public BattleModifier getModifier(ModifierType type)
        {
            return null;
        }

        public int index
        {
            get { return _index; }
        }

        public PlayerModifier modifier
        {
            get { return _modifier; }
        }

        public PlayerManna manna
        {
            get { return _manna; }
        }

        public Engine.BattleEngine engine
        {
            get { return (Engine.BattleEngine)Root; }
        }

        public virtual int race
        {
            get { return _race; }
        }

        public int id
        {
            get { return _id; }
        }
    }
}
