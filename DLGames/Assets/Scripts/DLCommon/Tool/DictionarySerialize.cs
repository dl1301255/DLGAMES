using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
	[System.Serializable]
	public struct DicObj 
	{
		public string key;
		public string value;
	}
	///<summary>
	///序列化 dic
	///<summary>
	public class DictionarySerialize:MonoSingleton<DictionarySerialize>
	{
		public static List<DicObj> SerializeDIC<K,V>(Dictionary<K,V> dic) 
		{
			List<DicObj>  dicObjs = new List<DicObj>();
			foreach (var item in dic)
			{
				DicObj obj = new DicObj();
				obj.key = item.Key.ToString();
				obj.value = item.Value.ToString();
				dicObjs.Add(obj);
			}
			return dicObjs;
		}
	}
}
