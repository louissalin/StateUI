using System.Collections.Generic;

namespace StateUI
{
	public class State : ContextOwner
	{
		public int Id { get; set; }
		public string Name { get; set; }
		
		public void InitContext()
		{
			if (Context != null)
				Context.Init(this);
		}
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
