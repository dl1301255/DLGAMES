using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class LockRot : MonoBehaviour
    {
        Quaternion quaternion;
        void Start()
        {
            quaternion = Quaternion.Euler(Vector3.zero);
        }
        void Update()
        {
            transform.rotation = quaternion;   
        }
    }
}
