using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Engine
{
    public class BattleContext
    {
        internal BattleConfiguration _configuration;
        internal BattleEngine _battleEngine;
        internal BattleActors _actors;
        internal BattleState _state;
        internal BattlePlayers _players;
        internal BattleOutput _output;

        public BattleConfiguration configuration
        {
            get { return _configuration; }
        }

        public BattleEngine battleEngine
        {
            get { return _battleEngine; }
        }

        public BattleActors actors
        {
            get { return _actors; }
        }

        public BattleState state
        {
            get { return _state; }
        }

        public BattlePlayers players
        {
            get { return _players; }
        }

        public BattleOutput output
        {
            get { return _output; }
        }
    }
}
