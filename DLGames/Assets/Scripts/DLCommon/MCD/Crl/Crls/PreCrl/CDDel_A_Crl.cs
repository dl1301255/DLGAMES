using DL.Common;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    public class CDDel_A_Crl : MCDCrlSO
    {
        public List<MCDMgr> mgrs;

        public override void Cloes()
        {
            base.Cloes();
            mgrs.Clear();
        }

        public override void Enable(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            if (mgr.GetUPC("UnitData")?.GetUP("CD_Del")?.GetSubType<UnitPropertyFloat>() == null) return;
            mgrs.Add(mgr);
        }


        [Button]
        public override void Init()
        {
            TimerManager.Instance.doLoop(this, 0, cdDel, TimeUpdataType.Update);
        }

        private void cdDel()
        {
            if (mgrs == null || mgrs.Count <= 0) return;

            for (int i = mgrs.Count - 1; i >= 0; i--)
            {
                if (mgrs[i] == null)
                {
                    mgrs.Remove(mgrs[i]);
                    continue;
                };
                var v = mgrs[i].GetUPC("UnitData")?.GetUP("CD_Del")?.GetSubType<UnitPropertyFloat>();
                if (v.Value <= 0) continue;
                v.Value -= Time.deltaTime;
            }

        }

        public override void Disable(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            mgrs.Remove(mgr);
        }
    }
}
