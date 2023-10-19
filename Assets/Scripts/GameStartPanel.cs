using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;

public class GameStartPanel : PanelBase
{
	void Start( )
	{
		Debug.Log("Start!!!!!");

		EventManager.Notify("fpsLock", true);
	}

	/// <summary>
	/// 点击开始游戏按钮
	/// </summary>
	public void OnClickStartButton( )
	{
		Debug.Log("OnClickCloseButton!!!!!!!!");

		EventManager.Notify("fpsLock", false);
		PanelManager.Inst.PopPanel(this);
	}
}