using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    /// 用于计数 包含del  和 max
    /// </summary>,float
    public class UnitPropertyCounterFloat : UnitProperty
    {
        public float MaxValue;
        public float ValueDel;

        public override void Init(MCDMgr mgr = null)
        {
            WriteValueByString(stringVal);
        }
        public override void ClearValue()
        {
            MaxValue = 0;
            ValueDel = 0;
        }

        public override string ReadValueByString()
        {
            stringVal = GameTool.GetStr(MaxValue.ToString(), ValueDel.ToString());
            return base.ReadValueByString();
        }

        public override void WriteValueByString(string val = null)
        {
            base.WriteValueByString(val);
            var v = GameTool.GetStringToArray(stringVal);
            MaxValue = float.Parse(v[0]);
            ValueDel = string.IsNullOrEmpty(v[1]) ? 0 : float.Parse(v[1]);
        }

        
    }
}
