using Ninject.Modules;
using NUnit.Framework;
using SpecUnit;

using StateUI;
using StateUI.NinjectBootStrapper;

namespace StateUISpecs.BootStrapper
{
	public class when_loading_the_main_module_with_a_root_context : MainModuleWithRootContext
	{
		[Observation]
		public void should_call_the_load_method_on_the_root_context()
		{
			rootContext.LoadExecuted.ShouldBeTrue();
		}
	}

	public class when_loading_the_main_module_with_child_contexts : MainModuleWithRootContext
	{
		[Observation]
		public void should_call_the_load_method_on_root_context_and_the_child_context_for_state_one()
		{
			rootContext.LoadExecuted.ShouldBeTrue();
			stateOneContext.LoadExecuted.ShouldBeTrue();
			stateTwoContext.LoadExecuted.ShouldBeFalse();
		}

		protected override void Context()
		{
			base.Context();

			stateOneContext = new MainContextStub(stateMachine, stateMachine.GetState(1));
			stateTwoContext = new MainContextStub(stateMachine, stateMachine.GetState(2));
			rootContext.AddChild(stateOneContext);
			rootContext.AddChild(stateTwoContext);
		}

		protected MainContextStub stateOneContext;
		protected MainContextStub stateTwoContext;
	}
	
	public class when_loading_the_main_module_after_having_changed_state : 
		MainModuleWithRootContext
	{
		[Observation]
		public void should_call_the_load_method_on_the_child_context_if_in_that_state()
		{
			childContext.LoadExecuted.ShouldBeTrue();
		}
		
		protected override void Context()
		{
			base.Context();

			childContext = new MainContextStub(stateMachine, stateMachine.GetState(2));
			rootContext.AddChild(childContext);

			stateMachine.ChangeState(2);
		}

		protected MainContextStub childContext;
	}

	[TestFixture]
	public class MainModuleWithRootContext : ContextSpecification
	{
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

			rootContext = new MainContextStub();
			sut = new MainModule(rootContext);
		}

		protected StateMachine stateMachine;
		protected MainContextStub rootContext;
		protected MainModule sut;
	}

	public class MainContextStub : Context
	{
		public bool LoadExecuted { get; set; }

		public MainContextStub(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
			LoadExecuted = true;
		}
	}
}
