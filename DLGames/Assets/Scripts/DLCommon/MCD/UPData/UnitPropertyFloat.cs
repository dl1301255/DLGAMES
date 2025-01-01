using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    public class UnitPropertyFloat : UnitProperty/*,IFloatValue*/
    {
        private float value;

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
#if UNITY_EDITOR
                ReadValueByString();
#endif
            }
        }


        public override string ReadValueByString()
        {
            StringVal = Value.ToString();
            return StringVal;
        }
        public override void WriteValueByString(string val = null)
        {
            base.WriteValueByString();
            if (string.IsNullOrEmpty(StringVal) && string.IsNullOrEmpty(val)) return;

            if (string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(StringVal))
            {
                Value = float.Parse(StringVal);
            }
            else
            {
                stringVal = val;
                Value = float.Parse(val);
            }

        }
        public override void ClearValue()
        {
            value = 0;
        }
        public override T GetValue<T>()
        {
            var v = GameCommonFactory.Instance.CreateObject<ParamsValue>("DL.Common.ParamsValue");
            v.FloatValue = value;
            return v as T;
        }


        public virtual float GetValue(string valueName = null)
        {
            return value;
        }
    }
}
