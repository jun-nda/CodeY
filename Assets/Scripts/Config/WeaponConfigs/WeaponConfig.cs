using System;
using System.Collections.Generic;
using UnityEngine;
using Common.UIScript;

namespace GameConfig
{
    /// <summary>
    /// 枪械配置基类
    /// </summary>
    public class WeaponConfig
    {
        // 武器名称
        public string name;

        // 武器类型
        public WeaponType weaponType;

        // 武器资源路径
        public string prefabPath;

        // 图片资源路径
        public string imagePath;


        //============武器数据相关配置=====================

        public float fireRate;

        public int magazineSize;

        public float damage;

        public int m_AmmoEach;

	    public int m_AmmoAll;

        public WeaponConfig(string name, WeaponType type, string prefabPath, string imagePath)
        {
            this.name = name;
            this.weaponType = type;
            this.prefabPath = prefabPath;
            this.imagePath = imagePath;
        }
    }
}