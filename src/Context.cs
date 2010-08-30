using Ninject.Modules;

namespace StateUI
{
	public interface IContext
	{
		void Init(IContextOwner owner);
	}

	public abstract class Context : NinjectModule, IContext
	{
		public bool ContextExecuted { get;set; }

		public void Init(IContextOwner owner)
		{
			owner.ContextExecuted = true;
		}
	}
}
