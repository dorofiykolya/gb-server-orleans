using BattleEngine.Records;

namespace BattleEngine.Players
{
    class BattleNPCPlayer : BattlePlayer
    {
        private BattleNPCRecord _npcPlayer;
		
		public BattleNPCPlayer(BattleNPCRecord npcPlayer)
        {
            _npcPlayer = npcPlayer;
        }

        override public int race 
		{
            get { return (int)Race.RACE_1; }
        }
}
}
