using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;

public class GameStartPanel : PanelBase
{
	void Start( )
	{
	}

	/// <summary>
	/// 点击开始游戏按钮
	/// </summary>
	public void OnClickStartButton( )
	{
		Debug.Log("OnClickCloseButton!!!!!!!!");

		EventManager.SendMessage(new GameStartEventArgs(true));
		PanelManager.Inst.PopPanel(this);
	}
}