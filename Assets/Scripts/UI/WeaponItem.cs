using System.Collections.Generic;
using GameData;
using UnityEngine;
using GameConfig;
using UnityEngine.UI;

public class WeaponItem : Toggle
{
	private WeaponConfig config;
	public void SetData(WeaponType t)
	{
		config = ConfigManager.Inst.GetWeaponByType(t);

		RefreshUI( );
	}

	void RefreshUI()
	{
		// 加载图片资源
		Debug.Log("imagePath = " + config.imagePath);
		Sprite weaponSprite = Resources.Load<Sprite>(config.imagePath);
		
		// 将图片显示在Image控件上
		Transform weaponTrasform = transform.Find("WeaponIcon");
		weaponTrasform.GetComponent<Image>().sprite = weaponSprite;
	}
}