using DL.Common;
using DL.MCD;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class CDCondition : ICondition
    {
        UnitProperty tempDelCD;

        public bool GetBool(MCD.MCDMgr mgr = null)
        {
            if (mgr.GetUPC("UnitData").TryGetUP("CD_Del", out tempDelCD)) 
            {
                if (tempDelCD.GetSubType<UnitPropertyFloat>().Value > 0) 
                {
                    return false;
                }
                return true;
            }

            return true;
        }

        public bool GetBool(MCD.MCDMgr mgr, object[] args = null)
        {
            throw new System.NotImplementedException();
        }

    }
}
