using System;
using System.Collections.Generic;

using Ninject.Modules;

using StateUI;

namespace StateUI.NinjectBootStrapper
{
	public abstract class Context
	{
		private StateMachine _stateMachine;
		private List<Context> _childContexts = new List<Context>();

		public bool IsValid 
		{ 
			get 
			{ 
				if (AssociatedState == null || _stateMachine == null) return true;
				return AssociatedState == _stateMachine.CurrentState; 
			} 
		}

		public State AssociatedState { get; private set; }
		public Context[] Children { get { return _childContexts.ToArray(); } }

		public Context()
		{
		}

		public Context(StateMachine stateMachine, State associatedState)
		{
			AssociatedState = associatedState;
			_stateMachine = stateMachine;
		}

		public void AddChild(Context child)
		{
			_childContexts.Add(child);
		}

		public abstract void Load(NinjectModule module);
	}
}
