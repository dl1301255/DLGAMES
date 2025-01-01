using DL.Common;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    ///默认 通过id 数据库中读取 读取顺序：baseData
    /// </summary>
    public class UPListUPC : UnitProperty
    {
        public List<UnitPropertyContainer> ListSkillUPC;

        public override void Init(MCDMgr mgr = null)
        {
            if (string.IsNullOrEmpty(StringVal)) return;
            ListSkillUPC = new List<UnitPropertyContainer>();

            // var upcNames = new List<string>(GameTool.GetStringToArray(StringVal));
            var upcNames = new List<string>(GameCommonFactory.Instance.GetStrAry(StringVal));

            for (int i = 0; i < upcNames.Count; i++)
            {
                var upc = UPCManager.Instance.GetBaseDataUPC(upcNames[i]);
                if (upc == null) continue;
                upc.Init(mgr);
                ListSkillUPC.Add(upc);
            }


        }

        //public override object Save()
        //{
        //    var list = new List<string>();
        //    ListSkillUPC.ForEach(s => list.Add(s.UPCName));
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in list)
        //    {
        //        sb.Append("_" + item);
        //    }
        //    StringVal = sb.ToString();
        //    return this;
        //}

        //public override void Load(object obj = null)
        //{
        //    base.Load(obj);
        //}

    }
}
