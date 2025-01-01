using DL.DLGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    public class FlickerEff_Crl : MCDCrlSO
    {
        ObjFlickerEff flickereff;
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;

            if (!mgr.gameObject.TryGetComponent(out flickereff)) 
            {
                mgr.gameObject.AddComponent<ObjFlickerEff>();
                flickereff = mgr.GetComponent<ObjFlickerEff>();
            }
            flickereff.Flicker();
        }
    }
}
