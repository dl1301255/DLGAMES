using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
	///<summary>
	///自动回收对象
	///<summary>
	public class AutoCollcetObject : MonoBehaviour
	{
		[Tooltip("自动回收时间")]
		public float delay;

		protected virtual void OnEnable()
        {
            collcet();
        }

        private void collcet()
        {
            GameObjectPool.Instance.CollectObject(gameObject, delay);
        }
    }
}
