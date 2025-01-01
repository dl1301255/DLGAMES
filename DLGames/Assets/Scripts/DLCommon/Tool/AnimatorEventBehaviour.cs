using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DL.Pool;

namespace DL.Common
{
	public delegate void del_CancelAnimaEvent(GameObject gameObject, string animaParam);
	///<summary>
	///动画事件行为 
	///<summary>
	public class AnimatorEventBehaviour : MonoBehaviour
	{
		//控制动画事件的触发时间，前后摇 = 攻击执行 - 攻击集中动画 - 攻击造成伤害 - 攻击结束 - 可执行下个指令
		public event Action attackHandler; //事件 行动
		public event del_CancelAnimaEvent del_CancelAnimaEvent;
		private Animator ani;
		//private CharacterStatus chStatus;
		private void Start()
		{
			ani = this.GetComponent<Animator>();
			//chStatus = GetComponentInParent<CharacterStatus>();
			del_CancelAnimaEvent += DeadCollectObj;
		}

		private void DeadCollectObj(GameObject gameObject, string animaParam)
		{
			if (animaParam != "dead") return;
			//GameObjectPool.Instance.CollectObject(GetComponentInParent<CharacterStatus>().gameObject);
		}

		/// <summary>
		/// Unity调用引擎调用 退出动画
		/// </summary>
		/// <param name="animParam">退出的动画Name</param>
		public void CancelAnima(string animParam) 
		{
			ani.SetBool(animParam, false);
			del_CancelAnimaEvent?.Invoke(this.gameObject, animParam);
		}
		public void OnAttack() //调用OnAttack(绑定到物体上 调用动画) ，判断是否有事件行动，如果有 则执行事件行动
		{
			attackHandler?.Invoke();//引发事件
		}

		//public void DeadOver(string animParam) //调用OnAttack(绑定到物体上 调用动画) ，判断是否有事件行动，如果有 则执行事件行动
		//{
		//	ani.SetBool(animParam, false);
		//	GameObjectPool.Instance.CollectObject(this.gameObject.transform.parent.gameObject);
		//}

		private void OnEnable()
		{
		}
	}
}
