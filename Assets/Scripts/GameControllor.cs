using System;
using UnityEngine;

public class GameControllor : MonoBehaviour
{
	[SerializeField] private InputManager inputManager;
	[SerializeField] private PlayerControllor playerControllor;

	private void Start()
    {
        Init();
    }

    public void Init()
	{
		playerControllor.Init(inputManager);
	}
}