using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    ///<summary>
    ///Ê±¼ä¿ØÖÆÆ÷²âÊÔ
    ///<summary>
    public class TM_Example:MonoBehaviour
    {
        public TimerNode timeNode;
        public float delay;
        public float NodeTimeScale;
        private void Start()
        {
            timeNode = TimerManager.Instance.doLoop(this, delay, shoot);
            NodeTimeScale = timeNode.timeScale;
        }

        private void shoot()
        {
            print(gameObject.name + "_:_" + Time.time);
        }
    }
}

