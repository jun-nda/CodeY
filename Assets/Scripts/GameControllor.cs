using System;
using UnityEngine;
using Common.UIScript;
using GameData;
using GameConfig;

public class GameControllor : MonoBehaviour
{
	//[SerializeField] private PlayerControllor playerControllor;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private GameObject InitImageBg;
	[SerializeField] private Camera MianCamera;

	///创建玩家背包
	private WeaponBackPack playerWeaponBackPack = new WeaponBackPack( );

	//private bool MianUIOpend = true;

	private void Start()
	{
		Init( );
		//StartGame(false);
	}

	public void Init()
	{
		//EventManager.Register("StartGame", StartGame);
		EventManager.AddListener<GameStartEventArgs>(StartGame);

		PanelManager.Inst.Init( );
		PanelManager.Inst.PushPanel<GameStartPanel>("UIPrefabs/GameStartPanel");

		ConfigManager.Inst.Init( );
		DataManager.Inst.Init( );
		SettingManager.Inst.Init( );

		InputManager.OnEscapeKeyDown += OnEscapeKeyDown;
		InitImageBg.SetActive(false);

		InputManager.OnAlpha1KeyDown += OnAlpha1KeyDown;
		InputManager.OnAlpha2KeyDown += OnAlpha2KeyDown;
	}

	public void StartGame(object eventData)
	{
		Debug.Log("============StartGame=============");
		CreatePlayer( );

		EventManager.SendMessage(new CaharacterPause(false));
		PanelManager.Inst.PushPanel<GameUI>("UIPrefabs/MainPanel").OnPush(playerWeaponBackPack);
	}

	public void CreatePlayer()
	{
		MianCamera.gameObject.SetActive(false);

		playerWeaponBackPack.AddWeapon(WeaponType.Weapon_AK);
		playerWeaponBackPack.AddWeapon(WeaponType.Weapon_PSM);

		//playerControllor.Init( );
		Debug.Log("=======CreatePlayer=======");
		var player = ResManager.InstantiateGameObjectSync("Prefabs/Player");
		player.transform.SetParent(this.transform);

		var script = player.GetComponent<PlayerControllor222>( );
		script.Init();

		// 重设主摄像机，将UIcamera渲染到主摄像机上
		PanelManager.Inst.ResetMainCamera(script.playerCamera);
	}

	public void OnEscapeKeyDown()
	{
		bool have = PanelManager.Inst.CheckHavePanel("SettingPanel");

		if (have == false)
			PanelManager.Inst.PushPanel<SettingPanel>("UIPrefabs/SettingPanel");
	}

	public void OnAlpha1KeyDown()
	{
		//Debug.Log("OnAlpha1KeyDown");
		playerWeaponBackPack.SelectWeapon(1);
	}

	public void OnAlpha2KeyDown()
	{
		//Debug.Log("OnAlpha2KeyDown");
		playerWeaponBackPack.SelectWeapon(2);
	}

	public void OnDestroy()
	{
	}
}