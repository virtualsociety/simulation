using Stateless;
using Stateless.Graph;
using System.Text;
using Vs.Simulation.Core.Interfaces;

namespace Vs.Simulation.Core
{
    public abstract class BaseStateMachine<TState, TTriggers> : IStateGraphs
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

        public string GetForceFeedbackGraph()
        {
            StateGraph graph = new StateGraph(Machine.GetInfo());
            StringBuilder sb = new StringBuilder();
            sb.Append("{  \"nodes\": [");
            foreach (var state in graph.States)
            {
                sb.Append($"{{\"id\": \"{state.Key}\",\"neighbours\": []}},\r\n");
            }
            sb.Length = sb.Length - 3; // remove last comma
            sb.Append("\r\n],\r\n");
            sb.Append("\"links\": [");
            foreach (FixedTransition link in graph.Transitions)
            {
                sb.Append($"{{\"source\": \"{link.SourceState.StateName }\",\"target\": \"{link.DestinationState.StateName }\",\"label\": \"{link.Trigger.ToString() }\"}},\r\n");
            }
            sb.Length = sb.Length - 3; // remove last comma
            sb.Append("\r\n]\r\n");
            sb.Append("}");
            return sb.ToString();
        }
    }
}
