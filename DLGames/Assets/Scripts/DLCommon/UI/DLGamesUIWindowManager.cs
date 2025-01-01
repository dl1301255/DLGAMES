using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class DLGamesUIWindowManager : UIWindowManager
    {
        public override void OnStart()
        {
            print("DLGamesUIWindowManager:" + name);
        }
    }
}

