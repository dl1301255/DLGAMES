using DL.DLGame;
using DL.MCD;
using DL.MCD.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    /// <summary>
    /// 跟踪mgr shoot 事件 
    /// 增加 额外的子弹 通过不同的方向进行附加射击,
    /// 额外射击 不属于 fire 防止循环引用 属于 低等级的射击
    /// </summary>
    public class EquipmentInitShoot_A_Eff : IEff
    {
        bool Inited = false;
        private string[] upPath = new string[2] { "UnitData", "ShootNum" };
        private string eventTag = "Fired";
        private List<MCDMgr> listMgr;
        private FireData crFD;
        private int defNum = 1;

        public void Init()
        {
            if (Inited) return;
            //init
            EventManager.Instance.ListenEvent<FireData>(eventTag, exe);
            listMgr = new List<MCDMgr>();
            Inited = true;
        }

        private void exe(FireData t1)
        {
            if (!Inited) Init();

            if (!listMgr.Contains(t1.FirerMgr)) return;

            crFD = t1;
            if (crFD == null) return;

            //尝试获取射击数  失败则使用默认
            var num = t1.FirerMgr.GetUP(upPath[0], upPath[1])?.GetValue<ParamsValue>().FloatValue;

            var name = t1.BulletMgr.name.GetStrReplace("(Clone)");

            if(num == null || num == 0)num = defNum;

            for (int i = 1; i < num + 1; i++)
            {
                var v3 = t1.BulletMgr.transform.rotation;
                v3.y += (0.1f * i);

                GameObjectPool.Instance.CreateObject(name, t1.BulletMgr.gameObject, t1.BulletMgr.transform.position, v3,
                null, initBullet);

                v3 = t1.BulletMgr.transform.rotation;
                v3.y += (-0.1f * i);

                GameObjectPool.Instance.CreateObject(name, t1.BulletMgr.gameObject, t1.BulletMgr.transform.position, v3,
                null, initBullet);
            }


        }

        private void initBullet(GameObject obj)
        {
            var mgr = obj.GetComponent<MCDMgr>();
            if (mgr == null) return;

            var bullet = obj.GetComponent<Bullet_DL>();
            bullet.ListUPC.Add(crFD.FirerMgr.ListUPC[0]);
            //for (int i = 0; i < mgr.ListUPC[0].ListUP.Count; i++)
            //{
            //    var v = crFD.BulletMgr.ListUPC[0].GetUP(mgr.ListUPC[0].ListUP[i].UPName);
            //    if (v == null) continue;
            //    mgr.ListUPC[0].ListUP[i].SetValue(v.GetValue<ParamsValue>());
            //}
        }

        public void Execute(object obj = null)
        {
            if (!Inited) Init();
            var mgr = obj as MCDMgr;
            if (mgr == null) return;
            if (!listMgr.Contains(mgr))
            {
                listMgr.Add(mgr);
            }
        }

        public void Execute(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            listMgr.Clear();
            crFD = null;
            Inited = false;
        }

        public void Close(object obj = null)
        {
            if (!Inited) Init();

            var mgr = (MCDMgr)obj;
            if (mgr == null) return;
            
            if (listMgr.Contains(mgr))
            {
                listMgr.Remove(mgr);
            }
        }

        public void Close(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public bool OnUse()
        {
            return Inited;
        }
    }

}
