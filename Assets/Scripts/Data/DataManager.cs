using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    /// <summary>
    /// 这个DataManager设计为了从服务器中获取数据
    /// </summary>
	public partial class DataManager : Singleton<DataManager>
	{
        // 不要了
        /// <summary>
        /// 根据武器的type获取武器的prefab路径
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns></returns>
		public string GetWeaponTypeString(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Weapon_AK:
                    return "Prefabs/Weapon_AK";
                case WeaponType.Weapon_PSM:
                    return "Prefabs/Weapon_PSM";
                default:
                    Debug.LogError("Unknown weapon type: " + weaponType);
                    return "";
            }
        }
	}
}