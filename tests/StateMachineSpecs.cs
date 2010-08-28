using System;

using NUnit.Framework;
using SpecUnit;

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
	}

	protected Path path;
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
