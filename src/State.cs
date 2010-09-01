using System.Collections.Generic;

namespace StateUI
{
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
}
