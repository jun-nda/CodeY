using System;
using UnityEngine;
using Common.UIScript;
using GameData;
public class GameControllor : MonoBehaviour
{
	[SerializeField] private PlayerControllor playerControllor;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private GameObject InitImageBg;

	///创建玩家背包
	private WeaponBackPack playerWeaponBackPack = new WeaponBackPack();

	//private bool MianUIOpend = true;

	private void Start( )
	{
		Init( );
		//StartGame(false);
	}

	public void Init( )
	{
		//EventManager.Register("StartGame", StartGame);
		EventManager.AddListener<GameStartEventArgs>(StartGame);

		PanelManager.Inst.Init( );
		PanelManager.Inst.PushPanel<GameStartPanel>("UIPrefabs/GameStartPanel");

		InputManager.OnEscapeKeyDown += OnEscapeKeyDown;
		InitImageBg.SetActive(false);

		InputManager.OnAlpha1KeyDown += OnAlpha1KeyDown;
		InputManager.OnAlpha2KeyDown += OnAlpha2KeyDown;
	}

	// TODO 在playerControllor里面把枪械，手臂模型摘出来，做成动态加载
	public void StartGame( object eventData )
	{
		Debug.Log("============StartGame=============");
		CreatePlayer( );

		EventManager.SendMessage(new CaharacterPause(false));
		PanelManager.Inst.PushPanel<GameUI>("UIPrefabs/MainPanel");
	}

	public void CreatePlayer( )
	{
		playerWeaponBackPack.AddWeapon(WeaponType.Weapon_AK);
		playerWeaponBackPack.AddWeapon(WeaponType.Weapon_PSM);

		playerControllor.Init( );
	}

	public void OnEscapeKeyDown( )
	{
		bool have = PanelManager.Inst.CheckHavePanel("SettingPanel");

		if ( have == false )
			PanelManager.Inst.PushPanel<MainPanel>("UIPrefabs/SettingPanel");
	}

	public void OnAlpha1KeyDown( )
	{
		//Debug.Log("OnAlpha1KeyDown");
		playerWeaponBackPack.SelectWeapon(1);
	}

	public void OnAlpha2KeyDown( )
	{
		//Debug.Log("OnAlpha2KeyDown");
		playerWeaponBackPack.SelectWeapon(2);
	}

	public void OnDestroy()
    {
    }
}