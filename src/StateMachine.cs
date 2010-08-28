using System;
using System.Collections.Generic;
using System.Linq;

public class StateMachine
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

		CurrentState = toState;
	}

	public State AddState(Action<State> initializer)
	{
		var state = new State { Id = _states.Count + 1 };
		_states.Add(state);

		if (_states.Count == 1)
			CurrentState = state;

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
}
