using StateUI;
using StateUI.NinjectBootStrapper;

using Ninject.Modules;

namespace ConsoleSamuraiApp
{
	public class App
	{
		public static void Main(string[] args)
		{
			var stateMachine = new StateMachine();
			stateMachine.AddState(s => s.Name = "UseSword");
			stateMachine.AddState(s => s.Name = "LoseSword");
			stateMachine.CreatePathFrom.State("UseSword").To.State("LoseSword");
			stateMachine.Start();

			var samuraiContext = new SamuraiContext();
			var mainModule = new MainModule(samuraiContext);
		}
	}

	public class SamuraiContext : Context
	{
		public SamuraiContext() {}

		public SamuraiContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
		}
	}

	public class UseSwordContext : Context
	{
		public UseSwordContext() {}

		public UseSwordContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
		}
	}

	public class UseBareHandsContext : Context
	{
		public UseBareHandsContext() {}

		public UseBareHandsContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
		}
	}
}
