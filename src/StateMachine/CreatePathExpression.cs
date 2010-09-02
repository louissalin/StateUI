using System;
using System.Collections.Generic;

namespace StateUI
{
	public class CreatePathExpression
	{
		private StateMachine _machine;
		private State _from;
		private State _to;
		private bool _selectingTo;
		private Path _createdPath;

		public CreatePathExpression To 
		{ 
			get 
			{ 
				_selectingTo = true;
				return this; 
			} 
		}

		public CreatePathExpression(StateMachine machine)
		{
			_machine = machine;
		}

		public CreatePathExpression State(int index)
		{
			var state = _machine.GetState(index);
			if (state == null)
				throw new ArgumentException(string.Format("There is no state with index {0}", index));

			if (_selectingTo)
			{
				_to = state;
				_createdPath = _machine.CreatePath(_from, _to);
				return this;
			}
			else
			{
				_from = state;
				return this;
			}
		}

		public Path GetPath()
		{
			return _createdPath;
		}
	}
}
