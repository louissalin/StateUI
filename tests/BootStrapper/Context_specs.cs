using System.Linq;

using Ninject.Modules;
using NUnit.Framework;
using SpecUnit;

using StateUI;
using StateUI.NinjectBootStrapper;

namespace StateUISpecs.BootStrapper
{
	public class when_add_child_contexts : ContextWithStateMachine
	{
		[Observation]
		public void should_add_the_child_to_the_root_context()
		{
			sut.Children.Any(c => c.AssociatedState.Id == 2).ShouldBeTrue();
		}
		
		protected override void Because()
		{
			childContext = new ContextStub(stateMachine, stateMachine.GetState(2));
			sut.AddChild(childContext);
		}

		protected ContextStub childContext;
	}

	public class when_associating_a_context_with_a_state : ContextWithStateMachine
	{
		[Observation]
		public void should_be_valid_if_we_are_in_that_state()
		{
			sut.IsValid.ShouldBeTrue();
		}

		[Observation]
		public void should_not_be_valid_if_we_are_not_on_that_state()
		{
			stateMachine.ChangeState(2);
			sut.IsValid.ShouldBeFalse();
		}
	}

	[TestFixture]
	public class ContextWithStateMachine : ContextSpecification
	{
		protected override void Context()
		{
			stateMachine = new StateMachine();
			stateMachine.AddState();
			stateMachine.AddState();
			stateMachine.CreatePathFrom.State(1).To.State(2);
			stateMachine.Start();

			sut = new ContextStub(stateMachine, stateMachine.GetState(1));
		}

		protected StateMachine stateMachine;
		protected ContextStub sut;
	}

	public class ContextStub : Context
	{
		public ContextStub(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module) {}
	}
}
