using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;

public class MainPanel : PanelBase
{
	void Start( )
	{
		Debug.Log("Start!!!!!");

		EventManager.Notify("fpsLock", true);
	}

	public void OnClickCloseButton( )
	{
		Debug.Log("OnClickCloseButton!!!!!!!!");

		EventManager.Notify("fpsLock", false);
		PanelManager.Inst.PopPanel(this);
	}
}