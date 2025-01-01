using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{

    public class UnitPropertyUPC : UnitProperty
    {
        private UnitPropertyContainer _value;

        public UnitPropertyContainer Value
        {
            get => _value;
            set
            {
                _value = value;
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(value.UPCName)) return;
                StringVal = value.UPCName;
#endif
            }
        }
    }
}
