using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class BulletDataSO : ScriptableObject
    {
        [Tooltip("持续时间")]
        public float Duration;
        public float Speed;
        [Tooltip("检测层")]
        public LayerMask TargetLayer;
    }
}
