using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;

public class MainPanel : PanelBase
{
	void Start( )
	{
		EventManager.Notify("fpsLock", true);
		Debug.Log("Start!!!!!");
	}

	public void OnClickCloseButton( )
	{
		EventManager.Notify("fpsLock", false);
		Debug.Log("OnClickCloseButton!!!!!!!!");
		PanelManager.Inst.PopPanel(this);
	}
}