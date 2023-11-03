using UnityEngine;
using UnityEngine.UI;
using Common.UIScript;
using GameData;

public class SettingPanel : PanelBase
{

	public SimpleSlider smoothingSlider;
	public Text smoothingValueText;

	private float smoothingValue;

	void Start( )
	{
		EventManager.SendMessage(new CaharacterPause(true));
		smoothingValue = SettingManager.Inst.GetSmoothing();
		smoothingValueText.text = smoothingValue.ToString();

		Debug.Log("smoothingValue = " + smoothingValue);

		smoothingSlider.SetData(smoothingValue, 10, 0, 1, k =>
		{
			smoothingValue = k;
			Debug.Log("smoothingValue = " + k);
			smoothingValueText.text = k.ToString();
		});
	}

	public void OnClickCloseButton( )
	{
		Debug.Log("OnClickCloseButton!!!!!!!!");
		
		SettingManager.Inst.SetSmoothing(smoothingValue);
		SettingManager.Inst.SaveSettingData();

		EventManager.SendMessage(new CaharacterPause(false));
		PanelManager.Inst.PopPanel(this);
	}

	/// <summary>
	/// 关闭面板时保存设置
	/// </summary>
	public void OnDestroy( )
	{
	}
}