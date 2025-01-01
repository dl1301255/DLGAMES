using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Storm
{
    public class LevelPart : MCD.MCDMgr
    {
        public bool Use;
        public int PowerLv;
        public override void Init()
        {

            if (!Use)
            {
                Use = true;
            }
        }
        public virtual void LpStart()
        {

        }
        public virtual void LpEnd()
        {

        }


    }
}
