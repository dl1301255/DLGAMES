using DL.Common;
using DL.MCD.Crl;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public class MCDMgr : MonoBehaviour
    {
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "UPCName")]
        [Searchable]
        public List<UnitPropertyContainer> ListUPC = new List<UnitPropertyContainer>();
        public List<MCDCrlSO> ListCrl;
        [ShowInInspector]
        public Dictionary<string, List<UnitPropertyContainer>> DicUPC;
        [ShowInInspector]
        public Dictionary<string, MCDCrlSO> DicCrl;

        MCDCrlSO tempCrl;
        UnitPropertyContainer tempUPC;
        bool Inited = false;


        private void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            if (Inited) return;

            UPCInit();
            DicUPC = new Dictionary<string, List<UnitPropertyContainer>>();
            DicCrl = new Dictionary<string, MCDCrlSO>();
            CrlInit();
            templist = new List<UnitPropertyContainer>();
            Inited = true;
        }

        protected virtual void UPCInit()
        {

            if (ListUPC.Count <= 0 || ListUPC == null) return;

            for (int i = 0; i < ListUPC.Count; i++)
            {
                ListUPC[i].Init(this);
            }
        }

        public virtual void UpdataDic()
        {
            foreach (var k in DicUPC.Keys)
            {
                templist = ListUPC.FindAll(s => s.UPCName == k || s.Tags.Contains(k));
                if (templist == null || templist.Count <= 0) continue;

                DicUPC[k].Clear();
                DicUPC.Add(k, templist);
                templist.Clear();
            }
        }

        public virtual MCDCrlSO GetCrl(string tag)
        {
            tempCrl = null;
            if (DicCrl.TryGetValue(tag, out tempCrl)) return tempCrl;

            tempCrl = ListCrl.Find(c => c.ID == tag || c.Tags.Contains(tag));
            if (tempCrl == null) return null;
            DicCrl.Add(tag, tempCrl);
            return tempCrl;
        }

        List<UnitPropertyContainer> templist;

        public virtual UnitPropertyContainer GetUPC(string upcName)
        {
            if (ListUPC.Count <= 0 || ListUPC == null || upcName == null) return null;

            if (DicUPC.TryGetValue(upcName, out templist))
            {
                if (templist.Count > 0)
                {
                    return templist[0];
                }
            }

            tempUPC = ListUPC.Find(s => s.UPCName == upcName || s.Tags.Contains(upcName));

            if (tempUPC == null) return null;
            else
            {
                DicUPC.Add(upcName, new List<UnitPropertyContainer>());
                DicUPC[upcName].Add(tempUPC);
                return tempUPC;
            }

        }
        public virtual UnitPropertyContainer GetUPC(int index)
        {
            if (ListUPC.Count <= 0 || ListUPC == null) return null;

            if (ListUPC[index] == null) return null;

            return ListUPC[index];

        }

        public virtual UnitProperty GetUP(string upcName, string upName, bool firstUpc = true)
        {
            if (ListUPC.Count <= 0 || ListUPC == null) return null;
            var v = firstUpc ? GetUPC(upcName)?.GetUP(upName) : GetUpByList(GetUPCs(upcName), upName);

            if (v == null) return null;
            return v;
        }

        /// <summary>
        /// 可能同一个upcname 可能有多个 返回第一个
        /// </summary>
        /// <param name="upcName"></param>
        /// <returns></returns>
        public virtual List<UnitPropertyContainer> GetUPCs(string upcName)
        {

            if (ListUPC.Count <= 0 || ListUPC == null || upcName == null) return null;

            if (DicUPC.TryGetValue(upcName, out templist))
            {
                if (templist.Count > 0)
                {
                    Debug.Log(templist.Count);
                    return templist;
                }
            }

            templist = ListUPC.FindAll(s => s.UPCName == upcName || s.Tags.Contains(upcName));

            if (templist == null) return null;

            else
            {
                DicUPC.Add(upcName, new List<UnitPropertyContainer>());

                for (int i = 0; i < templist.Count; i++)
                {
                    DicUPC[upcName].Add(templist[i]);
                }

                return templist;
            }
        }
        public virtual UnitProperty GetUpByList(List<UnitPropertyContainer> upc, string upName)
        {
            if (upc == null && upc.Count <= 0) return null;
            UnitProperty tempUp;
            for (int i = 0; i < upc.Count; i++)
            {
                tempUp = upc[i].GetUP(upName);
                if (tempUp == null) continue;
                return tempUp;
            }
            return null;
        }
        public virtual bool TryGetUP(string upcName, string upName, out UnitProperty up)
        {
            if (ListUPC.Count <= 0 || ListUPC == null)
            {
                up = null;
                return false;
            }

            up = GetUPC(upcName)?.GetUP(upName);
            if (up == null)
            {
                up = null;
                return false;
            }
            return true;
        }
        public virtual UnitProperty GetUP(string upName, int upcIndex = 0)
        {
            if (ListUPC.Count <= 0 || ListUPC == null) return null;
            var v = GetUPC(upcIndex)?.GetUP(upName);
            if (v == null) return null;
            return v;
        }

        public virtual bool TryGetUP<T>(string upName, out T value, int upcIndex = 0) where T : class
        {
            value = GetUP(upName) as T;
            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool TryGetUPC(string name, out UnitPropertyContainer upc, bool tagable = false)
        {
            var v = GetUPC(name);

            if (v != null)
            {
                upc = v;
                return true;
            }
            else
            {
                upc = null;
                return false;
            }
        }
        public virtual bool ContainUPC(string tag)
        {
            var upc = ListUPC.Find(s => s.Tags.Contains(tag) || s.UPCName == tag);
            if (upc == null) return false;
            return true;
        }

        public virtual void CrlInit()
        {
            if (ListCrl == null || ListCrl.Count <= 0) return;
        }
        protected virtual void Enable()
        {
            foreach (var c in ListCrl)
            {
                c.Enable(this);
            }
        }

        protected virtual void Disable()
        {
            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].Disable(this);
            }
        }

        public virtual void ExecuteCrl(string tag)
        {
            GetCrl(tag)?.ExecuteCrl(this);
        }

        public virtual void ExecuteCrl(string tag, params object[] args)
        {
            GetCrl(tag)?.ExecuteCrl(this, args);
        }

        protected virtual void OnEnable()
        {
            Enable();
        }

        protected virtual void OnDisable()
        {
            Disable();
        }

        public virtual void ExecuteCrl() { }
        public virtual void ExecuteCrl(params object[] args) { }

        public virtual void ExecuteCrl(int index = 0) { }
        public virtual void ExecuteCrl(int index, params object[] args) { }
        //缓存所有需要的数据 fire条件 or eff？ 
        //Crl 做两个一个自动 一个手动 

#if UNITY_EDITOR
        [Button]
        public void Reset()
        {
            if (this.gameObject.activeSelf)
            {
                for (int i = 0; i < ListCrl.Count; i++)
                {
                    ListCrl[i].Disable(this);
                }
                for (int i = 0; i < ListCrl.Count; i++)
                {
                    ListCrl[i].Enable(this);
                }
            }
        }

        [Button]
        public void isEnable()
        {
            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].Enable(this);
            }
        }
        [Button]
        public void isCrlDisable()
        {
            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].Disable(this);
            }
            print("isCrlDisable");
        }
#endif

    }
}
