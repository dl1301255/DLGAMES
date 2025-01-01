using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
	///<summary>
	///实例化 静态 唯一 方法工具
	///<summary>
	public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>//where 约束 要求传进来的T 必须是 MonoSingleton的子类
	{
		//public static T instance { get; private set; }
		//静态实例，一个必须的 唯一 脚本挂载在一个物体上 多用于必然属性 
		//物体+脚本 物体比如UI 比如GameManager ，对应上面应该有 ui.script 和 GameManager.script
		//脚本一定是提前写好的 所以创建失败 则可能是 实例物体没有创建 所以 我们 可以创建示例物体 在挂载上本上去 
		//实现静态类 3个步骤 ，1：创建物体 ， 2：挂脚本 ， 3 物体脚本连起来


		protected bool destoryOnLoad;

		private static T instance;//创建静态实例

		public static T Instance//为静态赋值
		{
			get 
			{

				if (instance == null) //如果 静态实例为空,上面没有脚本数据
				{
					instance = FindObjectOfType<T>();//为静态实例赋值 获取脚本数据
					if (instance == null) //如果静态实例赋值失败，则创建实例物体 为起赋值 获取脚本
					{
						instance = new GameObject("Singleton of " + typeof(T)).AddComponent<T>();//创建示例new GameObject，AddComponent，并且立即执行Awake
					}
					else 
					{
						if (instance.name == "MCDManager") Debug.Log("singleton Get not find");
						instance.Init();
					}
				}
				if (instance.name == "MCDManager") Debug.Log("singleton Get");
				return instance;
			}
			
		}
		
		public virtual void Awake()//没有物体 就执行
		{

			if (instance == null)//如果有实例物体 直接执行 获取脚本  
			{
				instance = this as T;
				destoryOnLoad = true;
				Init();
				if (instance.name == "MCDManager") Debug.Log("singleton Awake new");
			}
			if (instance.name == "MCDManager") Debug.Log("singleton Awake");
		}

		public virtual void Init() 
		{
			
		}//初始化

		public virtual T1 GetInstance<T1>() where T1 : T, new()
		{
			if (instance is T1)
			{
				return (T1)instance;
			}
			else return SetInstance<T1>();
		}

		public virtual T2 SetInstance<T2>() where T2 : T, new()
		{
			instance = new T2();
			return (T2)instance;
		}

		protected void DestoryOnLoad() 
		{
			if (!instance && destoryOnLoad) return;
            
			DontDestroyOnLoad(instance);
		}
    }
}
