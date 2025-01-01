using DL.Common;
using DL.Global;
using DL.MCD.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    public class ObjRigidbodyMove_A_Crl : MCDCrlSO, ICache
    {
        //ÒÆ¶¯
        [Sirenix.OdinInspector.ShowInInspector]
        public List<RgbMoveData> datas;
        private List<RgbMoveData> cache;
        public bool Useable = false;
        public float speed;

        ParamsValueVector value = new ParamsValueVector();
        public TimerNode fixedTN;


        public override void Init()
        {
            base.Init();
            cache = new List<RgbMoveData>();
            datas = new List<RgbMoveData>();
            fixedTN = TimerManager.Instance.doLoop(this, 0, move);
            Useable = true;
        }

        private void move()
        {
            for (int i = 0; i < datas.Count; i++)
            {
                var s = datas[i].speed == null ? speed : datas[i].speed.Value;
                if (datas[i].hv == null) return;
                value.vector2 = datas[i].hv.V2;
                datas[i].rgb.AddForce(value.vector2.x * s, 0, value.vector2.y * s, ForceMode.Force);
            }
        }

        public override void Enable(MCDMgr mgr)
        {
            if (!Useable)
            {
                Init();
            }
            if (datas.Find(s => s.mgr == mgr) == null)
            {
                RgbMoveData rmd;

                if (cache != null && cache.Count > 0)
                {
                    rmd = cache[0];
                }
                else
                {
                    rmd = new RgbMoveData();
                }

                rmd.mgr = mgr;
                rmd.hv = mgr.GetUP("HV") as UnitPropertyVector2;
                rmd.speed = mgr.GetUP("Speed") as UnitPropertyFloat;
                rmd.status = mgr.GetUP("Status");
                rmd.rgb = mgr.GetComponent<Rigidbody>();
                datas.Add(rmd);
            }

        }
        public override void Disable(MCDMgr mgr)
        {
            var d = datas.Find(s => s.mgr == mgr);
            if (d == null) return;
            datas.Remove(d);
            d.Clear();
            cache.Add(d);
        }

        public override void Cloes()
        {
            if (Useable)
            {
                Useable = false;
                datas.Clear();
                cache.Clear();
            }
        }

        private void OnDestroy()
        {
            if (Useable)
            {
                Useable = false;
                datas.Clear();
                cache.Clear();
            }
        }

        public void CacheClear()
        {
            cache.Clear();
        }

        [Serializable]
        public class RgbMoveData
        {
            public MCDMgr mgr;
            public Rigidbody rgb;
            public UnitPropertyVector2 hv;
            public UnitPropertyFloat speed;
            public UnitProperty status;
            public void Clear()
            {
                mgr = null;
                rgb = null;
                hv = null;
                speed = null;
            }
        }
    }
}
