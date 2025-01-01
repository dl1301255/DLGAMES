using DL.DLGame;
using DL.MCD;
using DL.MCD.Crl;
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
    /// 读取 shootNum；
    /// </summary>
    public class FanShape_SkillEff : IEff
    {
        bool Inited = false;
        private string[] upPath = new string[2] { "UnitData", "ShootNum" };
        private string eventTag = "FanShape_SkillEff";
        private int defNum = 1;

        private SkillData crSkillData;
        private GameObject crCacheSkillEntity;
        List<GameObject> listCacheCreateSkillEntity = new List<GameObject>();
        private float rotOffset = 0.1f;
        public void Init()
        {
            if (Inited) return;
            Inited = true;
        }

        private void exe(SkillData data)
        {

            crSkillData = data;
            if (crSkillData == null || crSkillData.SkillState != SkillStateEnum.Execute || crSkillData.InputType != SkillInputType.PointerDwon) return;

            //尝试获取射击数  失败则使用默认
            var num = data.SkillUPC.GetUP(upPath[1])?.GetValue<ParamsValue>().IntValue;
            if (num == null || num == 0) num = defNum;

            for (int i = data.ListSkillEntityMgr.Count - 1; i >= 0; i--)
            {
                createrSkill(data, i, num);
            }

            //skilled(data, num);

            EventManager.Instance.SendEvent(eventTag, data);
            reset();
        }

        private void createrSkill(SkillData data, int id, int? num)
        {
            var name = data.ListSkillEntityMgr[id].name;
            Quaternion v3;

            for (int i = 0; i < num; i++)
            {
                v3 = data.ListSkillEntityMgr[id].transform.rotation;
                if (!getRot(data, v3, out v3, 0, rotOffset)) return;

                crCacheSkillEntity = GameObjectPool.Instance.CreateObject(name, data.ListSkillEntityMgr[id].gameObject, data.ListSkillEntityMgr[id].transform.position, v3);

                data.ListSkillEntityMgr.Add(crCacheSkillEntity.GetComponent<MCDMgr>());

                v3 = data.ListSkillEntityMgr[id].transform.rotation;
                if (!getRot(data, v3, out v3, 0, -rotOffset)) return;

                crCacheSkillEntity = GameObjectPool.Instance.CreateObject(name, data.ListSkillEntityMgr[id].gameObject, data.ListSkillEntityMgr[id].transform.position, v3);

                data.ListSkillEntityMgr.Add(crCacheSkillEntity.GetComponent<MCDMgr>());
            }
        }

        private bool getRot(SkillData data, Quaternion v3, out Quaternion rot, int checkPosNum = 1, float offset = 0.1f)
        {
            v3.y += offset;

            for (int i = 0; i < data.ListSkillEntityMgr.Count; i++)
            {
                if (checkPos(v3, data.ListSkillEntityMgr[i].transform)) continue;
                else
                {
                    if (checkPosNum <= 10) getRot(data, v3, out v3, checkPosNum + 1, offset);

                    else
                    {
                        rot = v3;
                        return false;
                    }
                }
            }
            rot = v3;
            return true;
        }

        private bool checkPos(Quaternion v3, Transform target)
        {
            if (v3.y - 0.03f <= target.rotation.y && target.rotation.y <= v3.y + 0.03f) return false;
            else
            {
                return true;
            }
        }

        private void reset()
        {
            crSkillData = null;
            crCacheSkillEntity = null;
            listCacheCreateSkillEntity.Clear();
        }

        public void Execute(object obj = null)
        {
            if (!Inited) Init();
            exe(obj as SkillData);
        }

        public void Execute(params object[] obj)
        {

        }

        public void Close()
        {
            crSkillData = null;
            Inited = false;
        }

        public void Close(object obj = null)
        {

        }

        public void Close(params object[] obj)
        {

        }

        public bool OnUse()
        {
            return Inited;
        }
    }

}
