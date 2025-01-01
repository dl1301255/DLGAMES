using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using System;
using Sirenix.OdinInspector;

namespace DL.MCD.Crl
{
    /// <summary>
    /// 回收子弹 自动
    /// </summary>
    public class ObjPoolCollectCrl : MCDCrlSO
    {
        public bool Inited;
        public float defCollectTime;
        public string UpcName;
        public string UpSpeedName;
        public string UpRangeName;
        public List<BulletCollectCrlData> ListCollectData;
        public List<BulletCollectCrlData> cache;

        public bool Upcable = true;

        public override void Init()
        {
            if (Inited) return;
            ListCollectData = new List<BulletCollectCrlData>();
            cache = new List<BulletCollectCrlData>(10);
            TimerManager.Instance.doLoop(this, Time.deltaTime, onUpdateCollect, TimeUpdataType.Update);
            UpcName = "UnitData";
            UpSpeedName = "Speed";
            UpRangeName = "Range";
            Inited = true;
        }
        public override void Cloes()
        {
            Inited = false;
        }
        private void onUpdateCollect()
        {
            for (int i = ListCollectData.Count - 1; i >= 0; i--)
            {
                if (ListCollectData[i].CollectTime > Time.time) continue;
                else
                {
                    CollectData(ListCollectData[i]);
                }
            }
        }

        private void CollectData(BulletCollectCrlData data)
        {
            //发射事件 如果不希望目标被回收 则可以重新设置其启动
            ListCollectData.Remove(data);
            GameObjectPool.Instance.CollectObject(data.Mgr.gameObject);
            cache.Add(data);
            data.Clear();
        }

        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            if (!Inited) Init();

            if (!Upcable && defCollectTime == 0)
            {
                GameObjectPool.Instance.CollectObject(mgr.gameObject);
                return;
            }

            BulletCollectCrlData v;

            if (cache.Count <= 0)
            {
                v = new BulletCollectCrlData();
            }
            else
            {
                v = cache[0];
                cache.Remove(cache[0]);
            }
            v.Mgr = mgr;
            //获得目标的射程 和 速度 并计算回收时间
            v.CollectTime = getCollectTime(mgr);
            ListCollectData.Add(v);

        }

        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            ExecuteCrl(mgr);
        }

        private float getCollectTime(MCDMgr mgr)
        {
            if (!Upcable) return Time.time + defCollectTime;
            
            var s = mgr.GetUPC(UpcName)?.GetUP(UpSpeedName)?.GetSubType<UnitPropertyFloat>();
            var r = mgr.GetUPC(UpcName)?.GetUP(UpRangeName)?.GetSubType<UnitPropertyFloat>();

            if (r == null || s == null)
            {
                return Time.time +  defCollectTime;
            }
            else
            {
                return Time.time + (r.Value/s.Value);
            }
        }

        public override void Disable(MCDMgr mgr = null)
        {
            var data = ListCollectData.Find(s => s.Mgr == mgr);
            if (data == null)
            {
                return;
            }
            ListCollectData.Remove(data);
            data.Clear();
            cache.Add(data);
        }

        public override void Enable(MCDMgr mgr = null)
        {
            ExecuteCrl(mgr);
        }
    }

    [Serializable]
    public class BulletCollectCrlData
    {
        public MCDMgr Mgr;
        public float CollectTime;

        public void Clear()
        {
            Mgr = null;
            CollectTime = 0;
        }
    }
}
