using System;
using System.Collections.Generic;
using UnityEngine;
using Common.UIScript;

namespace GameData
{
	/// <summary>
	/// 玩家的背包列表,存的是武器的type TODO替换成weaponData
	/// </summary>
	public partial class WeaponBackPack
	{
		public List<WeaponType> weapons = new List<WeaponType>( );
		public int selectedWeaponIndex = 0;

		/// <summary>
		/// 想背包中添加武器
		/// </summary>
		public void AddWeapon( WeaponType weapon )
		{
			weapons.Add(weapon);
		}

		public void RemoveSelectedWeapon( )
		{
			if ( selectedWeaponIndex >= 0 && selectedWeaponIndex < weapons.Count )
			{
				weapons.RemoveAt(selectedWeaponIndex);

				if ( weapons.Count == 0 )
				{
					selectedWeaponIndex = -1;
				}
				else
				{
					selectedWeaponIndex = Mathf.Min(selectedWeaponIndex, weapons.Count - 1);
				}
			}
		}

		/// <summary>
		/// 选择武器，并发送事件
		/// </summary>
		public void SelectWeapon( int index )
		{
			if ( index > 0 && index <= weapons.Count )
			{
				selectedWeaponIndex = index - 1;
				EventManager.SendMessage(new ChangeWeapon(weapons[selectedWeaponIndex]));
			}
		}

		/// <summary>
		/// 选择下一把
		/// </summary>
		public void SelectNextWeapon( )
		{
			int nextIndex = ( selectedWeaponIndex + 1 ) % weapons.Count;
			SelectWeapon(nextIndex);
		}
		
		/// <summary>
		/// 选择上一把
		/// </summary>
		public void SelectPreviousWeapon( )
		{
			int previousIndex = ( selectedWeaponIndex - 1 + weapons.Count ) % weapons.Count;
			SelectWeapon(previousIndex);
		}
	}
}