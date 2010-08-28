using System;
using System.Collections.Generic;
using System.Linq;

public class State
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class Path
{
	private List<State> destinations = new List<State>();

	public State FromState { get; private set; }
	public State[] Destinations
	{
		get { return destinations.ToArray(); }
	}

	public Path(State fromState)
	{
		FromState = fromState;
	}

	public void AddDestination(State destination)
	{
		destinations.Add(destination);
	}
}

public class StateMachine
{
	private List<State> _states = new List<State>();
	private List<Path> _paths = new List<Path>();

	public bool HasState
	{
		get { return _states.Count > 0; }
	}

	public State StartState
	{
		get { return GetState(1); }
	}

	public CreatePathExpression CreatePathFrom
	{
		get { return new CreatePathExpression(this); }
	}

	public State AddState()
	{
		return AddState(null);
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
}

public class CreatePathExpression
{
	private StateMachine _machine;
	private State _from;
	private State _to;
	private bool _selectingTo;
	private Path _createdPath;

	public CreatePathExpression To 
	{ 
		get 
		{ 
			_selectingTo = true;
			return this; 
		} 
	}
	
	public CreatePathExpression(StateMachine machine)
	{
		_machine = machine;
	}

	public CreatePathExpression State(int index)
	{
		var state = _machine.GetState(index);
		if (state == null)
			throw new ArgumentException(string.Format("There is no state with index {0}", index));

		if (_selectingTo)
		{
			_to = state;
			_createdPath = _machine.CreatePath(_from, _to);
			return this;
		}
		else
		{
			_from = state;
			return this;
		}
	}

	public Path GetPath()
	{
		return _createdPath;
	}
}
