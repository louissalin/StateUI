using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateUI
{
	public class StateMachine : ContextOwner
	{
		private List<State> _states = new List<State>();
		private List<Path> _paths = new List<Path>();
		
		public bool HasState
		{
			get { return StateCount > 0; }
		}

		public int StateCount { get { return _states.Count; } }

		public State StartState
		{
			get { return GetState(1); }
		}

		public State CurrentState { get; private set; }

		public CreatePathExpression CreatePathFrom
		{
			get { return new CreatePathExpression(this); }
		}

		public State AddState()
		{
			return AddState(null);
		}

		public void ChangeState(int stateId)
		{
			if (GetState(stateId) == null)
				throw new ArgumentException(string.Format("State {0} does not exist", stateId));

			var path = _paths.SingleOrDefault(p => p.FromState == CurrentState);
			if (path == null)
				throw new Exception("There is no path to any other state");

			var toState = path.Destinations.SingleOrDefault(d => d.Id == stateId);
			if (toState == null)
				throw new ArgumentException(string.Format("There is no path from state {0} to state {1}", CurrentState.Id, stateId));

			SetCurrentState(toState);
		}

		public State AddState(Action<State> initializer)
		{
			var state = new State { Id = _states.Count + 1 };
			_states.Add(state);

			if (initializer != null)
				initializer(state);

			return state;
		}

		public State GetState(int id)
		{
			return _states.SingleOrDefault(s => s.Id == id);
		}

		public Path CreatePath(State from, State to)
		{
			if (from == to)
				throw new ArgumentException("Can't add a path between two equal states");

			Path path = _paths.SingleOrDefault(p => p.FromState == from);
			if (path == null)
			{
				path = new Path(from);
				_paths.Add(path);
			}

			if (path.Destinations.Any(d => d == to))
				throw new ArgumentException(string.Format("There is already a destination {0} for state {1}", to.Id, from.Id));

			path.AddDestination(to);
			return path;
		}

		public void Start()
		{
			if (Context == null)
				throw new Exception("There is no context, cannot start the state machine");

			if (_states.Count == 0)
				throw new Exception("There are no state in the state machine");

			Context.Init(this);
			SetCurrentState(1);
		}

		private void SetCurrentState(int stateId)
		{
			SetCurrentState(GetState(1));
		}

		private void SetCurrentState(State state)
		{
			CurrentState = state;
			CurrentState.InitContext();
		}

		public override string ToString()
		{
			var output = new StringBuilder();

			output.Append("\r\nStates...");
			foreach (var state in _states)
				output.AppendFormat("\r\nId: {0} -- Name: {1}", 
									 state.Id, 
									 state.Name ?? "no name");

			output.Append("\r\nPaths...");
			foreach (var path in _paths)
			{
				var dest = path.Destinations.Aggregate("", (s, i) => s + i.Id);
				output.AppendFormat("\r\n{0} --> {1}",
									 path.FromState.Id,
									 dest);
			}

			return output.ToString();
		}
	}
}
