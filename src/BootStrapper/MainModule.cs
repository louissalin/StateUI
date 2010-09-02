using Ninject.Modules;

namespace StateUI.NinjectBootStrapper
{
	public class MainModule :NinjectModule
	{
		private Context _rootContext;
		
		public MainModule(Context rootContext) : base()
		{
			_rootContext = rootContext;
		}

		public override void Load()
		{
			_rootContext.Load();
		}
	}
}
