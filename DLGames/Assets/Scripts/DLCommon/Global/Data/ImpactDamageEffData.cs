using DL.MCD;
using DL.MCD.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class ImpactDamageEffData : IData
    {
        public MCDMgr sourceMgr;
        public MCDMgr targetMgr;
        public float atk;
        public UnitPropertyFloat sourceAtk;
        public UnitPropertyFloat_A targetHp;
        public List<UnitProperty> ups ;
        public List<string> tag;
        public Collision coll;

        public T GetData<T>() where T : class
        {
            return this as T;
        }

        public void Init()
        {
            if (ups.ListIsNullorEmpty()) ups = new List<UnitProperty>();
        }

        public void Clear()
        {
            sourceMgr = null;
            targetMgr = null;
            atk = 0;
            sourceAtk = null;
            targetHp = null;
            if (!ups.ListIsNullorEmpty()) ups.Clear();
            coll = null;
            //if(tag != null)tag.Clear();
        }
    }
}