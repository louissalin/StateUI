using Ninject.Modules;

using StateUI;
using StateUI.NinjectBootStrapper;

namespace ConsoleSamuraiApp
{
	public class SamuraiContext : Context
	{
		public SamuraiContext() {}

		public SamuraiContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
			module.Bind<Samurai>().ToSelf();
		}
	}

	public class UseSwordContext : Context
	{
		public UseSwordContext() {}

		public UseSwordContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
			module.Bind<IWeapon>().To<Sword>();
		}
	}

	public class UseBareHandsContext : Context
	{
		public UseBareHandsContext() {}

		public UseBareHandsContext(StateMachine stateMachine, State associatedState)
			: base(stateMachine, associatedState)
		{
		}

		public override void Load(NinjectModule module)
		{
			module.Bind<IWeapon>().To<BareHands>();
		}
	}
}
