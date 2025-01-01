using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    [System.Serializable]
    public class SkillData
    {
        //表达skill状态 用于调用
        public SkillStateEnum SkillState;
        public SkillInputType InputType;
        // 释放技能的父单位
        public MCDMgr SourceMgr;
        //释放目标
        public List<MCDMgr> ListTargetMgr;
        //父单位 释放技能所使用的数据
        public UnitPropertyContainer SkillUPC;
        //技能skill entity 列表
        public List<MCDMgr> ListSkillEntityMgr = new List<MCDMgr>();

        public void Init(MCDMgr mgr, UnitPropertyContainer upc, List<MCDMgr> skillEntitys)
        {
            SourceMgr = mgr;
            SkillUPC = upc;
            ListSkillEntityMgr = skillEntitys;
        }

        public void Clear()
        {
            SourceMgr = null;
            SkillUPC = null;
            ListSkillEntityMgr.Clear();
            if (ListTargetMgr != null) ListTargetMgr.Clear();
        }
        
    }
    public enum SkillInputType 
    {
        PointerDwon,PointerUP,PointerHold,DoublePointer
            //可以包含组合按键 execrl跳过所有条件直接执行效果 并输入逻辑
    }
}
