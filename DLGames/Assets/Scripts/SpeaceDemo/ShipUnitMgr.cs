using DL.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    public class ShipUnitMgr : MCD.MCDMgr
    {
        int crlCount;

        protected override void Enable()
        {
            crlCount = ListCrl.Count;

            for (int i = 0; i < crlCount; i++)
            {
                ListCrl[i].Enable(this);
            }
        }

        protected override void Disable()
        {
            base.Disable();
        }


    }
}
