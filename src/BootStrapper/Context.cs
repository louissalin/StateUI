using StateUI;

namespace StateUI.NinjectBootStrapper
{
	public abstract class Context
	{
		private State _associatedState;
		private StateMachine _stateMachine;

		public bool IsValid 
		{ 
			get { return _associatedState == _stateMachine.CurrentState; } 
		}

		public Context(StateMachine stateMachine, State associatedState)
		{
			_associatedState = associatedState;
			_stateMachine = stateMachine;
		}

		public abstract void Load();
	}
}
