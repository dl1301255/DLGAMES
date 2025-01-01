using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    public class UnitPropertyBool : UnitProperty
    {
        private bool value;

        public bool Value 
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
            if (string.IsNullOrEmpty(StringVal)) return;
            // Value = bool.Parse(stringVal);
        }

    }
}
