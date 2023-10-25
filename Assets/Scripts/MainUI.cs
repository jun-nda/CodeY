using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;

public class MainPanel : PanelBase
{
	void Start( )
	{
		EventManager.SendMessage(new CaharacterPause(true));
	}

	public void OnClickCloseButton( )
	{
		Debug.Log("OnClickCloseButton!!!!!!!!");
		
		EventManager.SendMessage(new CaharacterPause(false));
		PanelManager.Inst.PopPanel(this);
	}

	public void OnDestroy( )
	{
		
	}
}