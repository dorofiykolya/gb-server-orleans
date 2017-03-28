namespace BattleEngine.Output
{
    public class BuildingChangeOwnerEvent : OutputEvent
    {
        public int objectId;
        public int ownerId;
        public int units;
    }
}
