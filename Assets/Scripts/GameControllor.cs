using System;
using UnityEngine;
using Common.UIScript;

public class GameControllor : MonoBehaviour
{
	[SerializeField] private PlayerControllor playerControllor;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private GameObject InitImageBg;

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
	}

	// TODO 在playerControllor里面把枪械，手臂模型摘出来，做成动态加载
	public void StartGame( object eventData )
	{
		Debug.Log("============StartGame=============");

		playerControllor.Init( );
		EventManager.SendMessage(new CaharacterPause(false));
		PanelManager.Inst.PushPanel<GameUI>("UIPrefabs/MainPanel");
	}

	public void OnEscapeKeyDown( )
	{
		bool have = PanelManager.Inst.CheckHavePanel("SettingPanel");

		if ( have == false )
			PanelManager.Inst.PushPanel<MainPanel>("UIPrefabs/SettingPanel");
	}
	 public void OnDestroy()
    {
    }
}