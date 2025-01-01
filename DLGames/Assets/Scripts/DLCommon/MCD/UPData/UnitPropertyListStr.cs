using DL.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DL.MCD.Data
{

    public class UnitPropertyListStr : UnitProperty
    {
        private List<string> value = new List<string>();

        public List<string> Value
        {
            get => value;
            set
            {
                this.value = value;
#if UNITY_EDITOR
                //StringVal = this.value.ToString();
#endif
            }
        }

        public override void WriteValueByString(string val = null)
        {
            if (string.IsNullOrEmpty(val)) return;
            
            if (value == null || value.Count <= 0)
            {
                value = new List<string>(GameCommonFactory.Instance.GetStrAry(val));
            }
            else
            {
                value.Clear();
                var vs = GameTool.GetStringToArray(val, ',');
                for (int i = 0; i < vs.Length; i++)
                {
                    if (value.Contains(vs[i])) continue;
                    else value.Add(vs[i]);               
                }
            }
        }

        public override string ReadValueByString()
        {
            return  GameTool.GetStr(',', value.ToArray());
        }

        public override void Init(MCDMgr mgr = null)
        {
            WriteValueByString(stringVal);
        }

        public override void ClearValue()
        {
            value.Clear();
        }
    }

}
