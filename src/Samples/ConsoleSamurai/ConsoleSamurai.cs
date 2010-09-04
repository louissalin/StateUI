using Ninject;

using StateUI;
using StateUI.NinjectBootStrapper;

namespace ConsoleSamuraiApp
{
	public class App
	{
		private IKernel _kernel;
		private MainModule _mainModule;
		private StateMachine _sm;

		public static void Main(string[] args)
		{
			var app = new App();
			app.Init();
			app.Run();
		}

		private void Init()
		{
			_sm = new StateMachine();
			_sm.AddState(s => s.Name = "UseSword");
			_sm.AddState(s => s.Name = "LoseSword");
			_sm.CreatePathFrom.State("UseSword").To.State("LoseSword");

			// when starting the state machine, the first state declared
			// is automatically loaded
			_sm.Start();

			var samuraiContext = new SamuraiContext();
			_mainModule = new MainModule(samuraiContext);

			var swordContext = new UseSwordContext(_sm, _sm.GetState("UseSword"));
			samuraiContext.AddChild(swordContext);

			var bareHandsContext = new UseBareHandsContext(_sm, _sm.GetState("LoseSword"));
			samuraiContext.AddChild(bareHandsContext);
		}

		private void Run()
		{
			LoadKernel();
			var samurai = _kernel.Get<Samurai>();
			samurai.Attack();

			_sm.ChangeState("LoseSword");
			LoadKernel();

			samurai = _kernel.Get<Samurai>();
			samurai.Attack();
		}

		private void LoadKernel()
		{
			_kernel = new StandardKernel(_mainModule);
		}
	}
}
