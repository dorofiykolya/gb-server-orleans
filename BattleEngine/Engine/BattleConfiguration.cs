using BattleEngine.Records;
using BattleEngine.Utils;
using Records.Buildings;
using Records.Modes;
using Records.Spells;
using Records.Units;

namespace BattleEngine.Engine
{
    public class BattleConfiguration
    {
        public BattleNPCRecord npcPlayer;

        public const int ticksPerSecond = 30;
        public const int processingSmothing = 100;
        public const int maxTicks = ticksPerSecond * 60 * 3;

        public readonly Vector<BattleBuildingRecord> buildings = new Vector<BattleBuildingRecord>();
        public readonly Vector<BattleOwnerRecord> owners = new Vector<BattleOwnerRecord>();
        public readonly Vector<BattleAction> actions = new Vector<BattleAction>();

        public UnitRecordMap unitRecords;
        public BuildingsRecordMap buildingRecords;
        public ModeRecordMap modeRecords;
        public SpellRecordMap spellRecords;
    }
}
