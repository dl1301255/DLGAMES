using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    [System.Serializable]
    public class DamageData
    {
        public GameObject SenderObj;
        public GameObject OwnerObj;
        public GameObject TargetObj;
        public MCD.Data.UnitPropertyContainer Upc;
        public float atk;
        public string atkType;

        public void Clear()
        {
            SenderObj = null;
            OwnerObj = null;
            TargetObj = null;
            Upc = null;
            atk = 0f;
            atkType = default;
        }
    }
}
