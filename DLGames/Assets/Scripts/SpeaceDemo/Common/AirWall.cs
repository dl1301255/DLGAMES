using DL.Common;
using DL.DLGame;
using DL.MCD;
using DL.MCD.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL
{
    public class AirWall : MonoBehaviour
    {
        public Rigidbody rgb;
        public MCDMgr mgr;

        private void stop(MCDMgr t1, ParamsValueVector t2)
        {
            if (t1 != mgr) return;
            
            var v = rgb.velocity;
            var p = transform.position - rgb.transform.position;

            if ((v.x >= 0 && p.x > 0 && t2.vector2.x > 0) || (v.x <= 0 && p.x < 0 && t2.vector2.x < 0))
            {
                t2.vector2.x = 0;
                rgb.velocity = new Vector3(0, v.y, v.z);
            }
            if ((v.z >= 0 && p.z > 0 && t2.vector2.y > 0) || (v.z <= 0 && p.z < 0 && t2.vector2.y < 0))
            {
                t2.vector2.y = 0;
                rgb.velocity = new Vector3(v.x, v.y, 0);
            }

            if ((v.z >= 0 && p.z > 0 && t2.vector3.z > 0) || (v.z <= 0 && p.z < 0 && t2.vector3.z < 0))
            {
                t2.vector3.z = 0;
                rgb.velocity = new Vector3(v.x, 0, v.z);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out mgr) && other.TryGetComponent(out rgb))
            {
                EventManager.Instance.ListenEvent<MCDMgr, ParamsValueVector>("PlayerMove", stop);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            EventManager.Instance.RemoveEvent<MCDMgr, ParamsValueVector>("PlayerMove", stop);
            rgb = null;
        }

    }
}
