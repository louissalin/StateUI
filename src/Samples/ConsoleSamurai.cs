using StateUI;
using StateUI.NinjectBootStrapper;

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
		public UseSwordContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
		}
	}
}
