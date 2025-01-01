using DL.Common;
using DL.MCD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class ImpactData
    {
        public Collision coll;
        public MCDMgr MgrA;
        public MCDMgr MgrB;

        internal void Clear()
        {
            coll = null;
            MgrA = null;
            MgrB = null;
        }
    }
}

