using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DL.Common
{
	///<summary>
	///配置文件读取器
	///<summary>
	public class ConfigurationReader
	{
		/// <summary>
		/// 获取配置文件 txt
		/// </summary>
		/// <param name="fileName">文件名称</param>
		/// <returns></returns>
		public static string GetConfigFile(string fileName) //获取配置文件
		{
			//获取 配置文件 StreamingAssets 文件夹中 配置文件
			//string url = "file://" + Application.streamingAssetsPath + "/"+ fileName;//file：// = 本地
			string url;

#if UNITY_EDITOR || UNITY_STANDALONE//编译器&PC下执行……
			url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;

#elif UNITY_IPHONE//否则 IOS下执行
			url = "file://" + Application.dataPath + "/Raw/"+fileName;
			
#elif UNITY_ANDROID//否则 安卓下执行……
			url = "jar:file://" + Application.dataPath + "!/assets/"+fileName;
#endif
			if (url == null) 
			{
				Debug.Log("没有配置文件");
				return null;
			}
			//加载 configMap 使用unityWabRequest 专门加载 本地 手机端文件

			return GameTool.ReadFile(url);
		}

		/// <summary>
		/// 文件读取器
		/// </summary>
		/// <param name="fileContent">文件内容</param>
		/// <param name="handler">读取方法(委托)</param>
		public static void Reader(string fileContent,Action<string> handler) 
		{
			//stirngReader = 字符串读取器
			using (StringReader reader = new StringReader(fileContent)) //程序执行，字符串读取器（读取filecontent）-这个读取器必须手动销毁
			{
				string line;
				while ((line = reader.ReadLine()) != null)//只要读取的信息 不为空 则执行
				{
					handler(line);
				}
			}
		}
	}
}
