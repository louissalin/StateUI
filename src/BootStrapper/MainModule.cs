using System.Collections.Generic;
using System.Linq;

using Ninject.Modules;

namespace StateUI.NinjectBootStrapper
{
	public class MainModule : NinjectModule
	{
		private Context _rootContext;
		
		public MainModule(Context rootContext) : base()
		{
			_rootContext = rootContext;
		}

		public override void Load()
		{
			_rootContext.Load(this);

			foreach(var child in ValidChildren())
				child.Load(this);
		}

		private IEnumerable<Context> ValidChildren()
		{
			return _rootContext.Children.Where(c => c.IsValid);
		}
	}
}
