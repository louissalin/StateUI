using System;

using NUnit.Framework;
using SpecUnit;

using StateUI;

namespace StateUISpecs
{
	public class when_using_a_new_state_machine : StateMachineSpec
	{
		[Observation]
			public void it_should_not_have_any_state()
			{
				sut.HasState.ShouldBeFalse();
			}

		[Observation]
			public void it_should_not_have_a_starting_state()
			{
				sut.StartState.ShouldBeNull();
			}
	}

	public class when_adding_the_first_state : StateMachineWithOneState
	{
		[Observation]
			public void it_should_have_a_state()
			{
				sut.HasState.ShouldBeTrue();
			}

		[Observation]
			public void it_should_have_an_id_of_one()
			{
				sut.GetState(1).ShouldNotBeNull();
			}

		[Observation]
			public void it_should_be_the_starting_state()
			{
				sut.GetState(1).ShouldEqual(sut.StartState);
			}
	}

	public class when_adding_other_states : StateMachineWithOneState
	{
		[Observation]
			public void should_increase_the_state_id_by_one_each_time()
			{
				sut.GetState(2).ShouldNotBeNull();
				sut.GetState(3).ShouldBeNull();
				sut.AddState();
				sut.GetState(3).ShouldNotBeNull();
			}

		protected override void Because()
		{
			sut.AddState();
			sut.AddState();
			sut.Start();
		}
	}

	public class when_specifying_states : StateMachineSpec
	{
		[Observation]
			public void should_be_able_to_name_the_state_during_the_add()
			{
				_state.Name.ShouldEqual(_name);
			}

		protected override void Because()
		{
			_state = sut.AddState(s => s.Name = _name);
			sut.Start();
		}

		protected State _state;
		protected string _name = "New Name";
	}

	public class when_creating_paths : StateMachineWithOneState
	{
		[Observation]
			public void should_have_create_a_path()
			{
				path.FromState.ShouldEqual(sut.GetState(1));
				path.Destinations.Length.ShouldEqual(1);
				path.Destinations[0].ShouldEqual(sut.GetState(2));
			}

		protected override void Because()
		{
			base.Because();

			sut.AddState();
			path = sut.CreatePathFrom.State(1).To.State(2).GetPath();
			sut.Start();
		}

		protected Path path;
	}

	public class when_creating_paths_from_the_same_state : StateMachineWithOneState
	{
		[Observation]
			public void should_have_create_a_path_with_two_destinations()
			{
				path.FromState.ShouldEqual(sut.GetState(1));
				path.Destinations.Length.ShouldEqual(2);
				path.Destinations[0].ShouldEqual(sut.GetState(2));
				path.Destinations[1].ShouldEqual(sut.GetState(3));
			}

		protected override void Because()
		{
			base.Because();

			sut.AddState();
			sut.AddState();
			sut.CreatePathFrom.State(1).To.State(2).GetPath();
			path = sut.CreatePathFrom.State(1).To.State(3).GetPath();

			sut.Start();
		}

		protected Path path;
	}

	public class when_creating_invalid_paths : StateMachineWithOneState
	{
		[Observation]
			public void should_throw_an_exception_when_creating_path_with_invalid_state()
			{
				typeof(ArgumentException).ShouldBeThrownBy(
						() => sut.CreatePathFrom.State(3))
					.ShouldContainErrorMessage("There is no state with index 3");

				typeof(ArgumentException).ShouldBeThrownBy(
						() => sut.CreatePathFrom.State(1).To.State(3))
					.ShouldContainErrorMessage("There is no state with index 3");
			}

		[Observation]
			public void should_throw_an_exception_when_creating_a_path_with_same_states()
			{
				typeof(ArgumentException).ShouldBeThrownBy(
						() => sut.CreatePathFrom.State(1).To.State(1))
					.ShouldContainErrorMessage("Can't add a path between two equal states");
			}
	}

	public class when_starting_up : StateMachineWithOneState
	{
		[Observation]
			public void should_set_the_current_state_to_the_starting_state()
			{
				sut.CurrentState.ShouldEqual(sut.StartState);
			}

		[Observation]
			public void should_execute_the_context_of_the_state_machine()
			{
				sut.ContextExecuted.ShouldBeTrue();
			}

		[Observation]
		public void should_execute_the_context_of_the_starting_state()
		{
			sut.CurrentState.ContextExecuted.ShouldBeTrue();
		}
		
		protected override void Because()
		{
			base.Because();

			sut.GetState(1).SetContext(new SpecContext());
			sut.Start();
		}
	}

	[TestFixture]
	public class when_starting_up_without_a_context : ContextSpecification
	{
		[Observation]
		public void should_throw_an_exception()
		{
			typeof(Exception).ShouldBeThrownBy(
					() => sut.Start())
				.ShouldContainErrorMessage("There is no context, cannot start the state machine");
		}

		protected override void Context()
		{
			sut = new StateMachine();
		}

		protected StateMachine sut;
	}

	public class when_changing_to_an_invalid_state : StateMachineWithOneState
	{
		[Observation]
			public void should_throw_an_exception_if_there_are_not_paths_from_the_current_state()
			{
				typeof(Exception).ShouldBeThrownBy(
						() => sut.ChangeState(2))
					.ShouldContainErrorMessage("There is no path to any other state");
			}

		[Observation]
			public void should_throw_an_exception_if_the_state_does_not_exist()
			{
				typeof(ArgumentException).ShouldBeThrownBy(
						() => sut.ChangeState(3))
					.ShouldContainErrorMessage("State 3 does not exist");
			}

		[Observation]
			public void should_throw_an_exception_if_the_state_does_not_have_a_path_to_the_new_state()
			{
				sut.AddState(s => s.Name = "state 3");
				sut.CreatePathFrom.State(1).To.State(3);

				typeof(ArgumentException).ShouldBeThrownBy(
						() => sut.ChangeState(2))
					.ShouldContainErrorMessage("There is no path from state 1 to state 2");
			}

		protected override void Because()
		{
			base.Because();
			sut.AddState(s => s.Name = "state 2");
			sut.Start();
		}
	}

	public class when_changing_to_a_valid_state : StateMachineWithOneState
	{
		[Observation]
			public void should_change_the_current_state_to_the_new_state()
			{
				sut.CurrentState.ShouldEqual(sut.GetState(2));
			}

		[Observation]
		public void should_execute_the_state_machine_context_and_state_context()
		{
			sut.ContextExecuted.ShouldBeTrue();
			sut.CurrentState.ContextExecuted.ShouldBeTrue();
		}

		protected override void Because()
		{
			base.Because();

			sut.AddState(s => s.Name = "state 2");
			sut.CreatePathFrom.State(1).To.State(2);
			sut.Start();
			sut.ChangeState(2);
		}
	}

	public class StateMachineWithOneState : StateMachineSpec 
	{
		protected override void Because()
		{
			sut.AddState();
		}
	}

	[TestFixture]
		public class StateMachineSpec : ContextSpecification
	{
		protected override void Context()
		{
			var context = new SpecContext();

			sut = new StateMachine();
			sut.SetContext(context);
		}

		protected StateMachine sut;
	}
}
