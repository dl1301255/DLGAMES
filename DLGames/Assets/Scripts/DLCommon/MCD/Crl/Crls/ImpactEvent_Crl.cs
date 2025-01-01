using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using System;
using System.Text;
using Sirenix.OdinInspector;

namespace DL.MCD.Crl
{
    public class ImpactEvent_Crl : MCDCrlSO
    {
        [Sirenix.OdinInspector.ShowInInspector]
        private string eventStr ;
        [Sirenix.OdinInspector.ShowInInspector]
        private string[] detonateEffsUpTag;
        [Sirenix.OdinInspector.ShowInInspector]
        private string[] DefEffs;
        public List<MCDCrlSO> ListDefObjHitCrl = new List<MCDCrlSO>();

        private StringBuilder sb = new StringBuilder();
        public bool useable;

        public override void Init()
        {
            if (!useable)
            {
                eventStr = "BulletDetonateEvent";
                EventManager.Instance.ListenEvent<ImpactData>(eventStr, onDetonateEvent);
                detonateEffsUpTag = new string[2] { "UnitData", "DetonateEffs" };
                DefEffs = new string[3] { "ImpactDamage_Eff", "ImpactExecuteFX_Eff", "CollectPoolObj_0_Eff" };
                sb.Clear();
                if (!ListDefObjHitCrl.ListIsNullorEmpty())
                {
                    for (int i = 0; i < ListDefObjHitCrl.Count; i++)
                    {
                        ListDefObjHitCrl[i].Init();
                    }
                }
         
                useable = true;
            }
        }
        public override void Cloes()
        {
            EventManager.Instance.RemoveEvent<ImpactData>(eventStr, onDetonateEvent);
            if (!ListDefObjHitCrl.ListIsNullorEmpty())
            {
                for (int i = 0; i < ListDefObjHitCrl.Count; i++)
                {
                    ListDefObjHitCrl[i].Cloes();
                }
            }
            useable = false;
        }
        private void onDetonateEvent(ImpactData t1)
        {
            exeObjAEffs(t1);
            exeDefEffs(t1);
            exeListDefObjHitCrl(t1);
        }

        private void exeListDefObjHitCrl(ImpactData t1)
        {
            if (ListDefObjHitCrl.ListIsNullorEmpty()) return;
            
            for (int i = 0; i < ListDefObjHitCrl.Count; i++)
            {
                ListDefObjHitCrl[i].ExecuteCrl(t1.MgrB);
            }
        }

        private void exeDefEffs(ImpactData t1)
        {
            exeEffs(DefEffs, t1);
        }

        private void exeObjAEffs(ImpactData t1)
        {
            var v = t1.MgrA.GetUP(detonateEffsUpTag[0], detonateEffsUpTag[1]);
            //Common工厂实现效果链

            if (v == null || string.IsNullOrEmpty(v.stringVal)) return;
            var effs = GameCommonFactory.Instance.GetStrAry(v.stringVal);
            exeEffs(effs, t1);
        }

        private void exeEffs(string[] effs, ImpactData t1)
        {
            for (int i = 0; i < effs.Length; i++)
            {
                sb.Append("DL.Common.").Append(effs[i]);
                GameCommonFactory.Instance.CreateMethod<IEff>(sb.ToString())?.Execute(t1);
                sb.Clear();
            }
        }
    }


}
