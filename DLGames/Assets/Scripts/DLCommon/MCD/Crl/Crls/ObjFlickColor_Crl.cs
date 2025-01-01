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
    public class ObjFlickColor_Crl : MCDCrlSO
    {
        public bool Usable;
        public bool returnColor;
        public AnimationCurve ac;
        public float durationTime;
        public Color flickColor;
        public List<ObjFlickColorData> ListData;
        public TimerNode tn;

        [Button]
        public override void Init()
        {
            if (Usable) return;
            tn = TimerManager.Instance.doLoop(this, 0, exe);
            ListData = new List<ObjFlickColorData>();
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
                for (int i = 0; i < v.renders.Count; i++)
                {
                    v.renders[i].materials[0].color = v.OGColors[i];
                }
                v.crTime = 0;
            }
            else
            {
                var fcd = GameCommonFactory.Instance.CreateObjectByPool<ObjFlickColorData>("DL.MCD.Crl.ObjFlickColorData");
                var vs1 = mgr.GetComponentsInChildren<Renderer>();
                var vs2 = mgr.GetComponentsInParent<Renderer>();
                if (vs1.Length <= 0) return;

                fcd.renders = new List<Renderer>(vs1);

                for (int i = 0; i < vs2.Length; i++)
                {
                    if (i == 0) continue;
                    fcd.renders.Add(vs2[i]);
                }

                for (int i = 0; i < fcd.renders.Count; i++)
                {
                    fcd.OGColors.Add(fcd.renders[i].materials[0].color);
                }

                fcd.mgr = mgr;
                fcd.crTime = 0;
                ListData.Add(fcd);
            }
        }

        Color crColor;
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
                    //Vector4 v = new Vector4(onAc * flickColor.r, onAc * flickColor.g, onAc * flickColor.b, 1f);

                    crColor.r = (onAc == 0f) ? 255 : onAc * flickColor.r;
                    crColor.g = (onAc == 0f) ? 255 : onAc * flickColor.g;
                    crColor.b = (onAc == 0f) ? 255 : onAc * flickColor.b;
                    crColor.a = 1f;
                    //Debug.Log(ListData[i].renders[0].materials[0].color);
                    for (int i1 = 0; i1 < ListData[i].renders.Count; i1++)
                    {
                        //ListData[i].renders[i1].materials[0].color = v;
                        ListData[i].renders[i1].materials[0].color = crColor;
                    }
                    ListData[i].crTime += Time.deltaTime;
                }
            }
        }
        private void endExe(ObjFlickColorData data)
        {
            ListData.Remove(data);
            //if (returnColor) data.render.materials[0].color = data.OGColor;
            if (returnColor)
            {
                for (int i = 0; i < data.renders.Count; i++)
                {
                    data.renders[i].materials[0].color = data.OGColors[i];
                }
            }

            data.Clear();
            GameCommonFactory.Instance.CollectObjectByPool(data);
        }
    }

    [System.Serializable]
    public class ObjFlickColorData
    {
        public MCDMgr mgr;
        public Vector4 OGColor;
        public float crTime;
        public Renderer render;
        public List<Vector4> OGColors = new List<Vector4>();
        public List<Renderer> renders;

        internal void Clear()
        {
            mgr = null;
            render = null;
            OGColor = Vector4.zero;
            crTime = 0f;
            OGColors.Clear();
        }
    }
}
