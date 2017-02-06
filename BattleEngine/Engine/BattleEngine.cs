using BattleEngine.Actions;
using BattleEngine.Output;
using BattleEngine.Providers;
using Common.Composite;

namespace BattleEngine.Engine
{
    public class BattleEngine : Entity
    {
        private BattleConfiguration _configuration;
        private BattleActionQueue _actionQueue;
        private BattleState _state;
        private BattleEngineProcessor _processor;
        private BattleActionEngine _actionEngine;
        private BattleActors _actors;
        private BattleContext _context;
        private BattleModulesProcessor _modules;
        private BattlePlayers _players;
        private BattleOutput _output;

        public BattleEngine(BattleConfiguration config)
        {
            _configuration = config;

            _context = new BattleContext();
            _context._configuration = _configuration;
            _context._battleEngine = this;

            _state = new BattleState(_configuration);
            _actionQueue = new BattleActionQueue(_configuration);
            _processor = new BattleEngineProcessor(_configuration, new BattleCommands());
            _actionEngine = new BattleActionEngine(_actionQueue);
            _actors = new BattleActors(this);
            _players = new BattlePlayers(_configuration, this);
            _output = new BattleOutput();
            _modules = new BattleModulesProcessor(_context, new BattleModules());

            _context._actors = _actors;
            _context._state = _state;
            _context._players = _players;
            _context._output = _output;

            _actionEngine.enqueue(new BattleStartAction());

            foreach (var action in config.actions)
            {
                _actionEngine.enqueue(action);
            }
        }

        public BattleContext context
        {
            get { return _context; }
        }

        public BattlePlayers players
        {
            get { return _players; }
        }

        public BattleOutput output
        {
            get { return _output; }
        }

        public BattleConfiguration configuration
        {
            get { return _configuration; }
        }

        public BattleState state
        {
            get { return _state; }
        }

        public int tick
        {
            get { return _state.tick; }
        }

        public BattleActionEngine actions
        {
            get { return _actionEngine; }
        }

        public int fastForward(int tick)
        {
            if (_state.isFinished)
            {
                return _state.tick;
            }

            var finish = false;
            if (tick >= _configuration.maxTicks)
            {
                tick = _configuration.maxTicks;
                finish = true;
            }
            var currentTick = _state.tick;
            while (++currentTick <= tick)
            {
                _state.updateTick(currentTick);
                _modules.preTick(_context, currentTick, _state.deltaTick);
                BattleAction currentAction;
                if (!_state.isFinished && _actionQueue.count > 0 && (currentAction = _actionQueue.peek()).tick < currentTick)
                {
                    _actionQueue.dequeue();
                    _processor.execute(currentAction, _context);
                }
                _modules.postTick(_context, currentTick, _state.deltaTick);
                if (_state.isFinished)
                {
                    break;
                }
            }
            if (!_state.isFinished)
            {
                _state.updateTick(tick);
                if (finish)
                {
                    output.enqueueByFactory<FinishEvent>(tick);
                    _state.finishBattle();
                }
            }
            return _state.tick;
        }
    }
}
