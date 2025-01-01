using DL.Common;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public class StormShipRotCrl : MCDCrlSO
    {
        /// <summary>
        /// Ã¿Ö¡Ðý×ª½Ç¶È
        /// </summary>
        public float speed;
        public float maxAngle;
        public UnitProperty tempUPvalue;
        Vector2 hv;
        Vector3 euler;
        public bool returnable;
        public float returnSpeed = 0.5f;
     

        Dictionary<MCDMgr, UnitProperty> cache = new Dictionary<MCDMgr, UnitProperty>();

        public override void Enable(MCDMgr mgr)
        {
            var v = (mgr as MCDMgr).GetUPC("Ship_001")?.GetUP("HV");
            if (v == null) return;
            cache.Add(mgr, v);
        }

        public override void Disable(MCDMgr mgr)
        {
            cache.Remove(mgr);
        }

        public  void OnFixedUpdata(MCDMgr mgr)
        {
            cache.TryGetValue(mgr, out tempUPvalue);
            if (tempUPvalue == null) return;
            if (returnable) objRotReturn(mgr);
            objRot(mgr);
            yaw(mgr);
            var pos = mgr.transform.position ;
            pos.y = 0;
            mgr.transform.position = pos;
        }

        private void objRotReturn(MCDMgr mgr)
        {
            hv = tempUPvalue.GetSubType<UnitPropertyVector2>().V2;
            if (hv.x != 0 && hv.y != 0) return;

            euler = mgr.transform.eulerAngles;

            float crEulerX = checkAngle(euler.x);
            float crEulerZ = checkAngle(euler.z);

            if (hv.y == 0)
            {
                if (crEulerX > -2f && crEulerX < 2f)
                {
                    setEuler(mgr, 0);
                    Debug.Log("setEuler:" + crEulerX);
                }
                else
                {
                    var y = (crEulerX > 0) ? -1f : 1f;
                    mgr.transform.Rotate(Vector3.right * y * speed * returnSpeed * Time.fixedDeltaTime, Space.World);
                    Debug.Log("Rotate");

                }
            }
            if (hv.x == 0)
            {
                //if (crEulerZ > -2f && crEulerZ < 2f)
                //{
                //    setEuler(mgr, null, null, 0);
                //}
                //else
                //{
                //    var y = (crEulerZ > 0) ? -1f : 1f;
                //    mgr.transform.Rotate(Vector3.forward * y * speed * returnSpeed * Time.fixedDeltaTime, Space.World);
                //}
            }
        }

        private void objRot(MCDMgr mgr)
        {
            hv = tempUPvalue.GetSubType<UnitPropertyVector2>().V2;
            if (hv.y == 0 && hv.x == 0) return;

            euler = mgr.transform.eulerAngles;

            float crEulerX = checkAngle(euler.x);
            float crEulerZ = checkAngle(euler.z);

            if (hv.y > 0 && crEulerX > maxAngle || crEulerX < -maxAngle && hv.y < 0) hv.y = 0;
            if (hv.x < 0 && crEulerZ > maxAngle || crEulerZ < -maxAngle && hv.x > 0) hv.x = 0;


            mgr.transform.Rotate(hv.y * speed * Time.fixedDeltaTime, 0, -hv.x * speed * Time.fixedDeltaTime, Space.World);
        }

        private float checkAngle(float v)
        {
            var f = v - 180;
            if (f > 0) return f - 180;
            else if (f == 0) return 0;
            else return f + 180;
        }
        private void yaw(MCDMgr mgr)
        {
            setEuler(mgr, null, 0, null);
        }
        private void setEuler(MCDMgr mgr, float? x = null, float? y = null, float? z = null)
        {
            euler = mgr.transform.eulerAngles;

            if (x != null)
            {
                euler.x = x.Value;
            }
            if (y != null)
            {
                euler.y = y.Value;
            }
            if (z != null)
            {
                euler.z = z.Value;
            }
            mgr.transform.rotation = Quaternion.Euler(euler);
        }
    }
}
