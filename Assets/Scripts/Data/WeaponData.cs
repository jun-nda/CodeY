namespace GameData
{
	/// <summary>
	/// 武器数据
	/// </summary>
	public partial class WeaponData
	{
		public int AmmoEach;		// 每个弹夹的弹药数量
		public int AmmoAll;			// 总共的弹药数量
		public int CurrentAmmo;		// 当前弹药数量
		public int CurrentAmmoAll;	// 当前弹药数量
		public float FireRate;		// 当前弹药数量

		public WeaponType weaponType;	// 武器类型

		public int Demage;			//伤害
		public int Recoil;          //后坐力系数

		/// <summary>
		///	构造函数
		/// </summary>
		/// <param name="_weaponType"></param>
		/// <param name="_AmmoEach"></param>
		/// <param name="_AmmoAll"></param>
		/// <param name="_FireRate"></param>
		public WeaponData(WeaponType _weaponType, int _AmmoEach, int _AmmoAll, float _FireRate)
		{
			weaponType = _weaponType;
			AmmoEach = _AmmoEach;
			AmmoAll = _AmmoAll;
			FireRate = _FireRate;

			CurrentAmmo = _AmmoEach;
			CurrentAmmoAll = _AmmoAll;
		}
	}
}