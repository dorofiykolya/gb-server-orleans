using BattleEngine.Utils;

namespace BattleEngine.Records
{
    public class BattleOwnerRecord
    {
        public int id;
        public string name;
        public int race;
        public Vector<BattleUnitRecord> units;
        public Vector<BattleSkillRecord> skills;
        public Vector<BattleModifierRecord> modifiers;
    }
}
