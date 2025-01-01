using DL.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    /// Tags[1]：UPCID 唯一码 保留
    /// </summary>
    //[InlineEditor]
    [System.Serializable]
    //[ShowInInspector]
    public class UnitPropertyContainer
    {
        public string UPCName;
        /// <summary>
        /// Tags[1]：UPCID 唯一码 保留
        /// UPC 根据tag dic 检索 
        /// </summary>
        public List<string> Tags = new List<string>();
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "UPName")]
        public List<UnitProperty> ListUP = new List<UnitProperty>();
        public Dictionary<string, UnitProperty> DicUP = new Dictionary<string, UnitProperty>();

        UnitProperty tempUp;

        /// <summary>
        /// 添加UP 名字相同会返回
        /// </summary>
        /// <param name="up"></param>
        public virtual void AddUP(UnitProperty up)
        {
            var v = ListUP.Find(i => up.UPName == i.UPName);

            if (v == null)
            {
                ListUP.Add(up);
            }
            else
            {
                return;
            }
        }
        //获得属性
        public virtual UnitProperty GetUP(string upName)
        {
            tempUp = null;

            if (DicUP.TryGetValue(upName, out tempUp)) return tempUp;
            
            tempUp = ListUP.Find(s => s.UPName == upName || s.tags.Contains(upName));

            if (tempUp == null) return null;
            //if (upName == "HP") Debug.Log(tempUP.GetType().Name);

            DicUP.Add(upName, tempUp);

            return tempUp;
        }

        UnitProperty tempUP;
        public virtual bool TryGetUP(string upName, out UnitProperty up)
        {
            tempUP = GetUP(upName);
            if (tempUP != null)
            {
                up = tempUP;
                return true;
            }
            else
            {
                up = null;
                return false;
            }
        }

        public virtual bool TryGetUP<T>(string upName, out T up) where T : UnitProperty
        {
            if (TryGetUP(upName, out tempUP))
            {
                up = tempUP as T;
                return true;
            }
            up = null;
            return false;
        }

        public virtual bool ContainsTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public virtual void Init(MCDMgr mgr = null)
        {

            if (ListUP.Count <= 0 || ListUP == null) return;

            var count = ListUP.Count;

            for (int i = 0; i < count; i++)
            {
                var u = UPCManager.Instance.UPInstance(ListUP[i], mgr);
                if (u == null) continue;
                ListUP[i] = u;
            }

        }

        public void Clear()
        {
            Tags.Clear();
            ListUP.Clear();
        }
    }


}
