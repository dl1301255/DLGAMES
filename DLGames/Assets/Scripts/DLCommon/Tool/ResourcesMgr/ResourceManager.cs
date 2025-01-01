using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DL.Common
{
	///<summary>
	///资源管理器 配置 读取
	///<summary>
	public class ResourceManager
	{
		private static Dictionary<string, string> configMap;

		/// <summary>
		/// 获取配置文件 & 传递解析方法
		/// </summary>
		static ResourceManager() //静态构造函数：初始化数据，类被加载时执行一次
		{
			configMap = new Dictionary<string, string>();
			//加载文件,获取文件 和 文件名
			string fileContent = ConfigurationReader.GetConfigFile("ConfigMap.txt");
			//解析文件 名字 = 路径 ：DIC  读取器(文件名内容，解析方法)
			ConfigurationReader.Reader(fileContent, BuildMap);
		}

		/// <summary>
		/// 解析方法
		/// </summary>
		/// <param name="line"></param>
		private static void BuildMap(string line)
		{
			string[] keyValue = line.Split('=');//信息分隔符 是 “=”
			configMap.Add(keyValue[0], keyValue[1]);//获取0,1 信息分别给到configmap（Key,Value）
		}
		

		//公开 静态 返回泛型 名字 泛型条件 （字符 名字）
		public static T Load<T>(string perfabName) where T : UnityEngine.Object
		{
			//dictionary的value返回方法 perfabName --> perfabPath
			if (configMap.ContainsKey(perfabName))
			{
				string perfabPath = configMap[perfabName];
				return Resources.Load<T>(perfabPath);//根据路径 返回游戏物体
			}
			return null;
		}
	}
}
