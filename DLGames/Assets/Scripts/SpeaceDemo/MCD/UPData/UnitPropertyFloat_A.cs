using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    public class UnitPropertyFloat_A : UnitPropertyFloat
    {
        //����Buff�Ӽ��� 
        private float _priorityValue;
        //����Buff�˷��ٷֱ�
        private float _overlayPerce;

        //�޸�ʱ ����value
        public float OverlayPerce
        {
            get => _overlayPerce;
            set => _overlayPerce = value;
        }
        //�޸�ʱ �ȼ� ��� ����value
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
