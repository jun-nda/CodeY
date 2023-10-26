using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
	private WeaponType weaponType;
	public void SetData( WeaponType t )
	{
		weaponType = t;
	}

	void RefreshUI( )
	{

	}
}