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
    public class ExeSkill_Crl : MCDCrlSO
    {
        public List<string> DefConditions ;
        public List<string> DefSkillEffs;
        public Vector3 DefSkillPoint;
        public SkillData CrSkillData = new SkillData();

        private UnitPropertyContainer crUpc;
        private MCDMgr crMgr;
        private GameObject go;
        private GameObject crSkillGo;
        private Transform skillPointTr;

        [Button]
        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            if (mgr == null) return;
            crMgr = mgr;
            crUpc = ps[0] as UnitPropertyContainer;
            if (crMgr == null) return;
            skillPointTr = ps[1] as Transform;
            if (skillPointTr == null) skillPointTr = crMgr.transform;


            //检测上次skill data ，输入为press or up double 的指令 会输入data
            //upc中加入一个input的string 使用静态
     

            //判断条件,是否创建new skill entity
            if (!onConditions(crMgr, crUpc)) return;

            CrSkillData.Clear();
            //创建skill
            if (!createSkillEntity(crMgr, crUpc, skillPointTr)) return;

            //计算CD & 重置数据
            reset();
        }

        private void reset()
        {
            crMgr = null;
            crUpc = null;
            go = null;
            crSkillGo = null;
            skillPointTr = null;
            //CrSkillData.Clear();
        }

        private bool createSkillEntity(MCDMgr mgr, UnitPropertyContainer upc, Transform skillPointTr)
        {
            if (!upc.TryGetUP<UnitProperty>(GameTool.GetStr("SkillPrefab"), out var up)) return false;
            if (string.IsNullOrEmpty(up.stringVal)) return false;

            go = AssetsManager.Instance.LoadAsset<GameObject>(up.stringVal);
            if (go == null) return false;

            DefSkillPoint += mgr.transform.position;
            crSkillGo = GameObjectPool.Instance.CreateObject(up.stringVal, go, skillPointTr.position + DefSkillPoint, skillPointTr.rotation);
            if (crSkillGo == null) return false;
     
            //播放效果列表
            exeSkillEffs(mgr, upc, crSkillGo);

            return true;
        }

        private void exeSkillEffs(MCDMgr crMgr, UnitPropertyContainer crUpc, GameObject go)
        {
            //创建Skilldata
            CrSkillData.SourceMgr = crMgr;
            CrSkillData.SkillUPC = crUpc;
            CrSkillData.ListSkillEntityMgr.Add(go.GetComponent<MCDMgr>());
            CrSkillData.SkillState = SkillStateEnum.Execute;

            if (DefSkillEffs != null && DefSkillEffs.Count > 0) onSkillEffs(CrSkillData, DefSkillEffs);

            var v = crUpc.GetUP(GameTool.GetStr("SkillEffs")) as UnitPropertyListStr;
            if (v == null || v.Value == null || v.Value.Count <= 0) return;
            onSkillEffs(CrSkillData, v.Value);
            CrSkillData.SkillState = SkillStateEnum.Update;
        }

        private void onSkillEffs(SkillData data, List<string> skillEffs)
        {
            for (int i = 0; i < skillEffs.Count; i++)
            {
                var sb = GameTool.GetStr("DL.Common.", skillEffs[i], "_SkillEff");
                GameCommonFactory.Instance.CreateMethod<IEff>(sb.ToString())?.Execute(data);
            }
        }


        private bool onConditions(MCDMgr crMgr, UnitPropertyContainer crUpc)
        {
            if (!conditions(crMgr, crUpc)) return false;
            if (!conditions(crMgr, crUpc)) return false;
            return true;
        }

        private bool conditions(MCDMgr crMgr, UnitPropertyContainer crUpc)
        {
            if (!crUpc.TryGetUP<UnitPropertyListStr>(GameTool.GetStr("SkillConditions"), out var up)) return true;
            
            //def and upc
            for (int i = 0; i < up.Value.Count; i++)
            {
                var sb = GameTool.GetStr("DL.Common.", DefConditions[i], "_SkillCondition");
                var v = GameCommonFactory.Instance.CreateMethod<ICondition>(sb.ToString())?.GetBool(crMgr, crUpc);
                if (v == false) return false;
            }
            return true;
        }
    }


}
