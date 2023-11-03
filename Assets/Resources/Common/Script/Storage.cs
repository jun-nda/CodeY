using System.IO;
using System;
using UnityEngine;

namespace Common.UIScript
{
    public class Storage
    {
        public static void SaveDataToFile<T>(T t, string fn)
		{
			// 将数据序列化为 JSON 格式的字符串
			string json = JsonUtility.ToJson(t);

			// 将 JSON 字符串写入文件
			File.WriteAllText(Application.dataPath + "/" + fn + ".json", json);
		}

		public static T ReadDataFromFile<T>(string fn)
		{
			// 读取本地文件中的 JSON 字符串
			string json = File.ReadAllText(Application.dataPath + "/" + fn + ".json");

			// 将 JSON 字符串反序列化为对象
			return JsonUtility.FromJson<T>(json);
		}
	}
}