using System;
using UnityEngine;
using Common.UIScript;

public class GameControllor : MonoBehaviour
{
	[SerializeField] private PlayerControllor playerControllor;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private GameObject InitImageBg;

	private void Start( )
	{
		Init( );
		StartGame(false);
	}

	public void Init( )
	{
		EventManager.Register("StartGame", StartGame);

		PanelManager.Inst.Init( );
		playerControllor.Init( );

		InputManager.OnEscapeKeyDown += OnEscapeKeyDown;
	}

	// TODO 游戏封面
	public void StartGame( object eventData )
	{
		InitImageBg.SetActive(false);
		PanelManager.Inst.PushPanel<GameStartPanel>("UIPrefabs/GameStartPanel");
	}

	public void OnEscapeKeyDown( )
	{
		PanelManager.Inst.PushPanel<MainPanel>("UIPrefabs/MainPanel");
	}

	 public void OnDestroy()
    {
    }
}