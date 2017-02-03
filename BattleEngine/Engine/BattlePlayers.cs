using System.Collections.Generic;
using BattleEngine.Players;
using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattlePlayers
    {
        private BattleEngine _battleEngine;
        private BattleConfiguration _configuration;

        private Dictionary<int, BattlePlayer> _map;
        private Vector<BattlePlayer> _players;
        private BattleNPCPlayer _npcPlayer;

        public BattlePlayers(BattleConfiguration configuration, BattleEngine battleEngine)
        {
            _configuration = configuration;
            _battleEngine = battleEngine;

            _map = new Dictionary<int, BattlePlayer>();
            _players = new Vector<BattlePlayer>();
            _npcPlayer = new BattleNPCPlayer(configuration.npcPlayer);
            initialize();
        }

        public bool isNPC(int ownerId)
        {
            return getPlayer(ownerId) == null;
        }

        public BattlePlayer getPlayer(int ownerId)
        {
            BattlePlayer result;
            if (!_map.TryGetValue(ownerId, out result))
            {
                result = _npcPlayer;
            }
            return result;
        }

        public Vector<BattlePlayer> players
        {
            get { return _players; }
        }

        private void initialize()
        {
            var index = 1;
            BattlePlayer player;
            foreach (var item in _configuration.owners)
            {
                _map[item.id] = player = new BattlePlayer();
                _battleEngine.AddComponent(player);
                _players.push(player);
                player.initialize(item, index);
                index++;
            }
            _battleEngine.AddComponent(_npcPlayer);
        }
    }
}
