using NUnit.Framework;
using SpecUnit;

using StateUI;
using StateUI.NinjectBootStrapper;

namespace StateUISpecs.BootStrapper
{
	[TestFixture]
	public class when_loading_the_main_module : ContextSpecification
	{
		[Observation]
		public void should_call_the_load_method_on_the_root_context()
		{
			rootContext.LoadExecuted.ShouldBeTrue();
		}

		protected override void Because()
		{
			sut.Load();
		}

		protected override void Context()
		{
			stateMachine = new StateMachine();
			stateMachine.AddState();
			stateMachine.AddState();
			stateMachine.CreatePathFrom.State(1).To.State(2);
			stateMachine.Start();

			rootContext = new RootContextStub(stateMachine, stateMachine.GetState(1));

			sut = new MainModule(rootContext);
		}

		protected StateMachine stateMachine;
		protected RootContextStub rootContext;
		protected MainModule sut;
	}

	public class RootContextStub : Context
	{
		public bool LoadExecuted { get; set; }

		public RootContextStub(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load()
		{
			LoadExecuted = true;
		}
	}
}
