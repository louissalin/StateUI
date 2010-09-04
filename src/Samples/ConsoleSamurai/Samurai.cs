namespace ConsoleSamuraiApp
{
	public class Samurai
	{
		private IWeapon _weapon;

		public Samurai(IWeapon weapon)
		{
			_weapon = weapon;
		}

		public void Attack()
		{
			System.Console.WriteLine(string.Format("Attacking with {0}", _weapon));
		}
	}

	public interface IWeapon {}

	public class Sword : IWeapon
	{
		public override string ToString()
		{
			return "a sword!";
		}
	}

	public class BareHands : IWeapon
	{
		public override string ToString()
		{
			return "my bare hands!";
		}
	}
}

