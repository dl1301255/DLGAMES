using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Pool
{
	///<summary>
	///生产迸发数字 by ObjPool
	///<summary>
	public class CreatePointNum : MonoBehaviour
	{
		string num;
		GameObject numObj;
		//获取角色事件
		private void Awake()
		{
			//numObj = ResourceManager.Load<GameObject>("PointNum");
		}

		private void OnEnable()
		{
			//chStatus.eveCharacterDamage += chDamageNum;
		}
		private void OnDisable()
		{
			//chStatus.eveCharacterDamage -= chDamageNum;
		}
		//将事件数字 输出


	}
}
