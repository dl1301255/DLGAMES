using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    /// 用于计数 包含del  和 max ,Int Type
    /// </summary>
    [System.Serializable]
    public class UnitPropertyCounterInt : UnitProperty
    {

        public int MaxValue;
        public int ValueDel;

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
            MaxValue = int.Parse(v[0]);
            ValueDel = string.IsNullOrEmpty(v[1]) ? 0 : int.Parse(v[1]);
        }
    }
}
