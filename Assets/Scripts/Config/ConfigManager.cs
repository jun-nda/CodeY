using System;
using System.Collections.Generic;
using UnityEngine;
using Common.UIScript;
using GameData;

namespace GameConfig
{
	public partial class ConfigManager : Singleton<ConfigManager>
	{
		private Dictionary<WeaponType, WeaponConfig> weaponConfigs = new Dictionary<WeaponType, WeaponConfig>();

		public void Init( )
		{
			InitWeaponConfigs( );
		}

		/// <summary>
		/// 初始化枪械配置
		/// </summary>
		public void InitWeaponConfigs( )
		{
			WeaponConfig ak47Config = new WeaponConfig(
				"AK47",
				WeaponType.Weapon_AK,
				"Prefabs/Weapon_AK",
				"Images/Weapon/AK47"
			);

			weaponConfigs.Add(WeaponType.Weapon_AK, ak47Config);

			WeaponConfig PSMConfig = new WeaponConfig(
				"PSM", 
				WeaponType.Weapon_PSM,
				"Prefabs/Weapon_PSM",
				"Images/Weapon/PSM"
			);

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