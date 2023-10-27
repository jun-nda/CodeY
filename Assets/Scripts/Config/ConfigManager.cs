using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameConfig
{
	public partial class ConfigManager : Singleton<ConfigManager>
	{
		//private List<WeaponConfig> weaponConfigs;
		private Dictionary<WeaponType, WeaponConfig> weaponConfigs;

		public void Init( )
		{
			InitWeaponConfigs( );
		}

		/// <summary>
		/// 初始化枪械配置
		/// </summary>
		public void InitWeaponConfigs( )
		{
			WeaponConfig ak47Config = new WeaponConfig( );
			ak47Config.name = "AK47";
			ak47Config.prefabPath = "Prefabs/Weapon_AK";
			ak47Config.weaponType = WeaponType.Weapon_AK;
			ak47Config.fireRate = 0.1f;
			ak47Config.magazineSize = 30;
			ak47Config.damage = 30;

			weaponConfigs.Add(WeaponType.Weapon_AK, ak47Config);

			WeaponConfig PSMConfig = new WeaponConfig( );
			PSMConfig.name = "AK47";
			PSMConfig.prefabPath = "Prefabs/Weapon_PSM";
			PSMConfig.weaponType = WeaponType.Weapon_PSM;
			PSMConfig.fireRate = 0.1f;
			PSMConfig.magazineSize = 30;
			PSMConfig.damage = 30;

			weaponConfigs.Add(WeaponType.Weapon_PSM, PSMConfig);
		}

		/// <summary>
		/// 根据WeaponType获取配置信息
		/// </summary>
		public WeaponConfig GetWeaponByType( WeaponType type)
		{
			if ( weaponConfigs.ContainsKey(type) )
			{
				return weaponConfigs[type];
			}

			return null;
		}
	}
}