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

	public class when_getting_states_by_name : StateMachineWithOneState
	{
		[Observation]
		public void should_allow_to_get_state_by_name()
		{
			sut.GetState("Two").ShouldEqual(sut.GetState(2));
		}

		[Observation]
		public void should_allow_to_change_state_by_name()
		{
			sut.ChangeState("Two");
			sut.CurrentState.ShouldEqual(sut.GetState(2));
		}

		[Observation]
		public void should_throw_an_exception_when_using_the_wrong_name()
		{
			typeof(ArgumentException).ShouldBeThrownBy(
					() => sut.CreatePathFrom.State("bla"))
				.ShouldContainErrorMessage("There is no state with name bla");

			sut.GetState("bla").ShouldBeNull();

			typeof(ArgumentException).ShouldBeThrownBy(
					() => sut.ChangeState("bla"))
				.ShouldContainErrorMessage("State 'bla' does not exist");
		}

		protected override void Because()
		{
			base.Because();
			sut.AddState(s => s.Name = "Two");
			sut.CreatePathFrom.State(1).To.State("Two");
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

		protected override void Because()
		{
			base.Because();
			sut.Start();
		}
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
			sut = new StateMachine();
		}

		protected StateMachine sut;
	}
}
