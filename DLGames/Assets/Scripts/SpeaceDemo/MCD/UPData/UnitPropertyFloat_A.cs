using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    public class UnitPropertyFloat_A : UnitPropertyFloat
    {
        //运算Buff加减法 
        private float _priorityValue;
        //运算Buff乘法百分比
        private float _overlayPerce;

        //修改时 调整value
        public float OverlayPerce
        {
            get => _overlayPerce;
            set => _overlayPerce = value;
        }
        //修改时 先减 后加 调整value
        public float PriorityValue
        {
            get => _priorityValue;
            set => _priorityValue = value;
        }

        public override string ReadValueByString()
        {
            StringVal = Value.ToString();
            return StringVal;
        }

        public override void WriteValueByString(string val = null)
        {

            if (stringVal == null) return;

            var arr = GameTool.GetStringToArray(stringVal);
            //if (v == null || v.Length < 3) return;

            if (arr.Length > 0) Value = float.Parse(arr[0]);
            if (arr.Length > 1) _priorityValue = float.Parse(arr[1]);
            if (arr.Length > 2) _overlayPerce = float.Parse(arr[2]);
        }

        public override float GetValue(string valueName = null)
        {
            if (valueName == "perce")
            {
                return _overlayPerce;
            }
            return Value;
        }
    }
}
