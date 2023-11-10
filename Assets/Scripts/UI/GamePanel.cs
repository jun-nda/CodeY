using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;
using GameData;

public class GameUI : PanelBase
{
	public GameObject WeaponBackPackContainer;
	public GameObject WeaponBackPackItem;
	public GameObject SightBead;
	public ToggleGroup toggleGroup;
	public GameObject frontSight;

	private readonly float itemScale = 1;

	private WeaponBackPack weaponBackPack;

	private float sightSize = 1f;
	private float curSize = 1f;
	private float walkTargetSize = 2f;
	private float runTargetSize = 2.5f;

	void Start()
	{
		/// 换枪事件
		EventManager.AddListener<ChangeWeapon>(OnChangeWeapon);
		
		// 瞄准事件
		EventManager.AddListener<AimingEventArgs>(OnAiming);
		// 背包的枪械换掉时需要重新刷一遍列表
	}

	public void OnPush(WeaponBackPack playerWeaponBackPack)
	{
		//playerWeaponBackPack.weapons;
		weaponBackPack = playerWeaponBackPack;
		Refresh( );
	}

	/// <summary>
	/// 准星动态变化
	/// </summary>
	public void Update()
	{
		float caharacterSpeed = DataManager.Inst.CaharacterSpeed;
		//Debug.Log("caharacterSpeed =  " + caharacterSpeed);
		if (caharacterSpeed > 0.02f && caharacterSpeed < 0.045f)
		{
			if (curSize == walkTargetSize)
				return;

			curSize = Mathf.Lerp(curSize, walkTargetSize, Time.deltaTime * 5);
		}
		else if (caharacterSpeed > 0.045f)
		{
			if (curSize == runTargetSize)
				return;

			curSize = Mathf.Lerp(curSize, runTargetSize, Time.deltaTime * 5);
		}
		else
		{
			curSize = Mathf.Lerp(curSize, sightSize, Time.deltaTime * 5);
		}

		RectTransform rectTransform = frontSight.GetComponent<RectTransform>( );
		rectTransform.sizeDelta = new Vector2(100 * curSize, 100 * curSize);
	}

	void Refresh()
	{
		for (int i = 0; i < weaponBackPack.weapons.Count; i++)
		{
			var go = Instantiate(WeaponBackPackItem, WeaponBackPackContainer.transform, false);
			go.transform.localScale = new Vector3(itemScale, itemScale, itemScale);

			var script = go.GetComponent<WeaponItem>( );
			script.SetData(weaponBackPack.weapons[i]);
		}

		toggleGroup.SetAllTogglesOff( ); // 取消所有 Toggle 的选中状态
	}

	public void OnChangeWeapon(ChangeWeapon eventData)
	{
		int index = weaponBackPack.weapons.IndexOf(eventData.WeaponType);

		toggleGroup.SetAllTogglesOff( ); // 取消所有 Toggle 的选中状态
		Transform toggleTransform = WeaponBackPackContainer.transform.GetChild(index);
		WeaponItem toggleToSelect = toggleTransform.GetComponent<WeaponItem>( );
		toggleToSelect.isOn = true; // 设置指定 Toggle 的选中状态为 true
	}

	public void OnAiming(AimingEventArgs eventData)
	{
		bool isAiming = eventData.IsAiming;
		SightBead.SetActive(!isAiming);
	}

	public void OnDestroy()
	{

	}
}