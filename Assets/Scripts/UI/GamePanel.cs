using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;
using GameData;

public class GameUI : PanelBase
{
    public GameObject WeaponBackPackContainer;
    public GameObject WeaponBackPackItem;
    public ToggleGroup toggleGroup;

    private float itemScale = 1;

	private WeaponBackPack weaponBackPack;

	void Start( )
	{
		/// 换枪事件
		EventManager.AddListener<ChangeWeapon>(OnChangeWeapon);
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
			var go = Instantiate(WeaponBackPackItem, WeaponBackPackContainer.transform, false);
            go.transform.localScale = new Vector3(itemScale, itemScale, itemScale);

			var script = go.GetComponent<WeaponItem>( );
			script.SetData(weaponBackPack.weapons[i]);
		}

		toggleGroup.SetAllTogglesOff(); // 取消所有 Toggle 的选中状态
	}

	public void OnChangeWeapon(ChangeWeapon eventData)
	{
		int index = weaponBackPack.weapons.IndexOf(eventData.WeaponType);
		Debug.Log("index = " + index);
		toggleGroup.SetAllTogglesOff(); // 取消所有 Toggle 的选中状态
		Transform toggleTransform = WeaponBackPackContainer.transform.GetChild(index);
		WeaponItem toggleToSelect = toggleTransform.GetComponent<WeaponItem>();
		toggleToSelect.isOn = true; // 设置指定 Toggle 的选中状态为 true
	}

	public void OnDestroy( )
	{
		
	}
}