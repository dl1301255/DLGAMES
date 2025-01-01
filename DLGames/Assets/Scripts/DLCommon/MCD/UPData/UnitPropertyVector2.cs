using DL.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DL.MCD.Data
{

    public class UnitPropertyVector2 : UnitProperty
    {
        private Vector2 v2;

        public Vector2 V2
        {
            get => v2;
            set
            {
                v2 = value;
#if UNITY_EDITOR
                StringVal = v2.ToString();
#endif
            }
        }

        public override void WriteValueByString(string val = null)
        {
            if (val == null) return;

            //var v = GameTool.GetStringToArray(val);
            var v = GameCommonFactory.Instance.GetStrAry(val);
            v2.x = float.Parse(v[0]);
            v2.y = float.Parse(v[1]);
        }

        public override string ReadValueByString()
        {
            var srt1 = v2.x.ToString();
            var srt2 = v2.y.ToString();
            StringBuilder sb = new StringBuilder();
            return sb.Append(srt1).Append("_").Append(srt2).ToString();
        }

        public override void ClearValue()
        {
            v2 = Vector2.zero;
        }

    }

}
