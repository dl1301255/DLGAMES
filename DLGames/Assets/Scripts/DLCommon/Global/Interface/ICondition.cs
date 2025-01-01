using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public interface ICondition
    {
        public bool GetBool(MCD.MCDMgr mgr = null);
        public bool GetBool(MCD.MCDMgr mgr, params object[] ps);
    }
}
