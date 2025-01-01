using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using System.Text;
using System;

namespace DL.MCD.Crl
{
    public class DeathEvent_A_Crl : MCDCrlSO
    {
        [Sirenix.OdinInspector.ShowInInspector]
        private string eventStr;
        [Sirenix.OdinInspector.ShowInInspector]
        private string[] deathEffsUpTag;
        public string[] DefEffs;
        public List<MCDCrlSO> ListDefObjHitCrl = new List<MCDCrlSO>();

        private StringBuilder sb = new StringBuilder();
        public bool useable;

        public override void Init()
        {
            if (!useable)
            {
                eventStr = "Unit_HP_0";
                EventManager.Instance.ListenEvent<ImpactData>(eventStr, eve);
                deathEffsUpTag = new string[2] { "UnitData", "DeathEffs" };

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

        private void eve(ImpactData t1)
        {
            exeDefEff(t1);
            exeCrls(t1.MgrB);
        }

        private void exeDefEff(ImpactData t1) 
        {
            for (int i = 0; i < DefEffs.Length; i++)
            {
                var eff = GameCommonFactory.Instance.CreateMethod<IEff>(GameTool.GetStr("DL.Common." + DefEffs[i]));

                if (eff == null) return;
                eff.Execute(t1);
            }
        }

        public override void Cloes()
        {
            EventManager.Instance.RemoveEvent<ImpactData>(eventStr, eve);
            for (int i = 0; i < ListDefObjHitCrl.Count; i++)
            {
                ListDefObjHitCrl[i].Cloes();
            }
            useable = false;
        }

        private void exeCrls(MCDMgr mgr) 
        {
            for (int i = 0; i < ListDefObjHitCrl.Count; i++)
            {
                ListDefObjHitCrl[i].ExecuteCrl(mgr);
            }
        }


    }

}
