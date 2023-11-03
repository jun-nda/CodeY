using System;
using System.IO;
using Common.UIScript;
using UnityEngine;

namespace GameData
{
	[Serializable]
	public class Setting
	{
		public float smoothing = 2.0f;
		public float musicVolume = 1.0f;
		// 其他设置项...
	}

	/// <summary>
	/// SettingManager 用来管理玩家设置
	/// </summary>
	public partial class SettingManager : Singleton<SettingManager>
	{
		private Setting _settings;

		/// <summary>
		/// 初始化加载设置
		/// </summary>
		public void Init()
		{
			if (File.Exists(Application.dataPath + "/FpsSetting.json"))
			{
				LoadSettingData( );
			}
			else
			{
				_settings = new( );
			}
		}

		public float GetSmoothing()
		{
			return _settings.smoothing;
		}

		public void SetSmoothing(float _smoothing)
		{
			_settings.smoothing = _smoothing;
		}

		public void SaveSettingData()
		{
			string fileName = "FpsSetting";
			Storage.SaveDataToFile(_settings, fileName);
		}

		/// <summary>
		/// 加载本地文件中的数据
		/// </summary>
		public void LoadSettingData()
		{
			string fileName = "FpsSetting";
			_settings = Storage.ReadDataFromFile<Setting>(fileName);
		}
	}
}