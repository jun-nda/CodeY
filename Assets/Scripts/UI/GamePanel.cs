using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;
using GameData;

public class GameUI : PanelBase
{
	
    public GameObject WeaponBackPackContainer;
    public GameObject WeaponBackPackItem;

    private float itemScale = 1;

	private WeaponBackPack weaponBackPack;

	void Start( )
	{

	}

	public void OnPush( WeaponBackPack playerWeaponBackPack )
	{
		//playerWeaponBackPack.weapons;
		weaponBackPack = playerWeaponBackPack;
		Refresh( );
	}

	void Refresh( )
	{
		for ( int i = 0 ; i < weaponBackPack.weapons.Count ; i++ )
		{
			var go = Instantiate(WeaponBackPackItem, WeaponBackPackContainer.transform);
            go.transform.localScale = new Vector3(itemScale, itemScale, itemScale);

			WeaponItem item = go.GetComponent<WeaponItem>();
			item.SetData(weaponBackPack.weapons[i]);
		}
	}

	public void OnDestroy( )
	{
		
	}
}