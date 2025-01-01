using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using Sirenix.OdinInspector;
using System;

namespace DL.MCD.Crl
{
    public class ObjShake_Crl : MCDCrlSO
    {
        public bool Usable;
        public bool returnPos;
        public AnimationCurve ac;
        public float durationTime;
        public Vector3 shakeIntensity;                                      //shake  Ç¿¶È
        public List<ShakeCrlData> ListData;
        public TimerNode tn;

        [Button]
        public override void Init()
        {
            base.Init();
            if (Usable) return;
            
            tn = TimerManager.Instance.doLoop(this, 0, exe);
            ListData = new List<ShakeCrlData>();
            Usable = true;
        }
        [Button]
        public override void Cloes()
        {
            if (!Usable) return;   
            ListData.Clear();
            TimerManager.Instance.Clear(tn);
            tn = null;
            Usable = false;
        }
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null || !Usable) return;

            var v = ListData.Find(s => s.mgr == mgr);

            if (v != null)
            {
                v.mgr.transform.position = v.OGV3;
                v.crTime = 0;
            }
            else
            {
                var scd = new ShakeCrlData();
                scd.mgr = mgr;
                scd.crTime = 0;
                scd.OGV3 = mgr.transform.position;
                ListData.Add(scd);
            }
        }

        Vector3 v;
        private void exe()
        {
            for (int i = 0; i < ListData.Count; i++)
            {
                if (ListData[i].crTime > durationTime)
                {
                    endExe(ListData[i]);
                    continue;
                }
                else
                {
                    var onAc = ac.Evaluate(ListData[i].crTime / durationTime);
                    //v = new Vector3(onAc * shakeIntensity.x, onAc * shakeIntensity.y, onAc * shakeIntensity.z);
                    v.x = onAc * shakeIntensity.x;
                    v.y = onAc * shakeIntensity.y;
                    v.z = onAc * shakeIntensity.z;

                    ListData[i].mgr.transform.Translate(v);
                    ListData[i].crTime += Time.deltaTime;
                }
            }
        }
        private void endExe(ShakeCrlData data)
        {
            ListData.Remove(data);
            if (returnPos) data.mgr.transform.position = data.OGV3;
            data.Clear();
        }
    }

    [System.Serializable]
    public class ShakeCrlData 
    {
        public MCDMgr mgr;
        public Vector3 OGV3;
        public float crTime;

        internal void Clear()
        {
            mgr = null;
            OGV3 = Vector3.zero;
            crTime = 0f;
        }
    }
}
