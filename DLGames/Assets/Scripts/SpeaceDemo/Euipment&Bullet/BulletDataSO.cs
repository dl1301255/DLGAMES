using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class BulletDataSO : ScriptableObject
    {
        [Tooltip("����ʱ��")]
        public float Duration;
        public float Speed;
        [Tooltip("����")]
        public LayerMask TargetLayer;
    }
}
