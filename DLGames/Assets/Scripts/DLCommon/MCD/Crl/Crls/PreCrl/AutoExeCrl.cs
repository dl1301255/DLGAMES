using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    public class AutoExeCrl : MCDCrlSO
    {
        public List<MCDMgr> mgrs;
        public string exeCrlName;

        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            mgrs.Add(mgr);
        }
        public void OnUpdate(MCDMgr mgr = null)
        {
            for (int i = 0; i < mgrs.Count; i++)
            {
                mgrs[i].GetCrl(exeCrlName).ExecuteCrl(mgrs[i]);
            }
        }


    }
}
