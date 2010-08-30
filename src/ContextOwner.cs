namespace StateUI
{
	public interface IContextOwner
	{
		bool ContextExecuted { get; set; }
		void SetContext(IContext context);
	}

	public class ContextOwner : IContextOwner
	{
		public IContext Context { get; private set; }
		public bool ContextExecuted { get; set; }

		public void SetContext(IContext context)
		{
			Context = context;
		}
	}
}
