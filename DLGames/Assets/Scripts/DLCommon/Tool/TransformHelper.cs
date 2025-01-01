using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
	///<summary>
	///变换组件助手类：用于查找子类目 未知名称 组件，递归
	///<summary>
	public static class TransformHelper
	{
		/// <summary>
		/// 变换组件助手类：用于查找子类目 未知名称 组件
		/// </summary>
		/// <param name="currentTF">当前Transform组件</param>
		/// <param name="childName">后代物体的名称</param>
		/// <returns></returns>
		public static Transform FindChildByName(this Transform currentTF, string childName) 
		{
			//查找
			Transform childTF = currentTF.Find(childName);//在当前CurrentTF中找到 childName 组件 交给 childTF ；
			if (childTF != null) return childTF;//找到就返回

			//将任务交给子物体 继续查找
			for (int i = 0; i < currentTF.childCount; i++)
			{
				childTF = FindChildByName(currentTF.GetChild(i), childName);//将任务交给子物体
				if (childTF != null) return childTF;
			}
			return null;
		}

		public static Transform FindParentByName(this Transform currentTF, string parentName)
		{
			Transform targetParentTF;
			//查找所有  子物体 
			Transform[] ParentTFs = currentTF.gameObject.GetComponentsInParent<Transform>();
			//遍历子物体 获取名字
			targetParentTF = ParentTFs.Find(s => s.name == parentName);
			if (targetParentTF != null) return targetParentTF;
			return null;
		}

		public static Transform[] FindChildsByName(this Transform currentTF, string childName)
		{
			//List<Transform> targetChildTFs = new List<Transform>();
			Transform[] targetChildTFs; 
			//查找所有  子物体 
			Transform[] childTFs = currentTF.gameObject.GetComponentsInChildren<Transform>();
			//遍历子物体 获取名字
			targetChildTFs = childTFs.FindAll(s => s.name == childName);
			return targetChildTFs;
		}

	}
}
