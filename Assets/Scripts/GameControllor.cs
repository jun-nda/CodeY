using System;
using UnityEngine;
using Common.UIScript;

public class GameControllor : MonoBehaviour
{
	[SerializeField] private PlayerControllor playerControllor;
	[SerializeField] private InputManager inputManager;

	private void Start( )
	{
		Init( );
		StartGame( );
	}

	public void Init( )
	{
        PanelManager.Inst.Init( );
		playerControllor.Init(inputManager);
		InputManager.OnEscapeKeyDown += OnEscapeKeyDown;
	}

	public void StartGame( )
	{
		//PanelManager.Inst.PushPanel<MainPanel>("UIPrefabs/MainPanel");
	}

	public void OnEscapeKeyDown( )
	{
		PanelManager.Inst.PushPanel<MainPanel>("UIPrefabs/MainPanel");
	}

	 public void OnDestroy()
    {
    }
}