using Stateless;
using Stateless.Graph;

namespace Vs.Simulation.Core
{
    public abstract class BaseStateMachine<TState, TTriggers>
    {
        public StateMachine<TState, TTriggers> Machine { get; private set; }
        private TState _state;

        public BaseStateMachine(TState initialState)
        {
            _state = initialState;
            Machine = new Stateless.StateMachine<TState, TTriggers>(() => _state, s => _state = s);
        }

        public string GetDiGraph()
        {

            return UmlDotGraph.Format(this.Machine.GetInfo());
        }
    }
}