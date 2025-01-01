using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;

namespace DL.MCD.Crl
{
    public class LPEnemeyCreaterCrl : MCDCrlSO
    {
        
    }

    [System.Serializable]
    public class UnitEnemyCreaterData
    {
        public string Name;
        public int CreatableNum;
        public float Stength;
        public UnitPropertyContainer Upc;
        public GameObject obj;

        public void Clear()
        {
            CreatableNum = 0;
            Stength = 0;
            Upc = null;
        }
    }
}
