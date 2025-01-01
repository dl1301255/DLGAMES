using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    public class AutoExecuteSkill_B : MCDCrlSO
    {
        private Dictionary<MCDMgr, UPListUPC> dicCache = new Dictionary<MCDMgr, UPListUPC>();
        private List<UnitPropertyContainer> upcs;

        public override void Enable(MCDMgr mgr = null)
        {
            //var v = (mgr as MCDMgr).GetUPC_Tag("UnitShip").GetUP("Skill_B_Bar") as UPListUPC;
            //dicCache.Add(mgr, v);
        }

        public override void Disable(MCDMgr mgr)
        {
            dicCache.Remove(mgr);
        }

        public  void OnFixedUpdata(MCDMgr mgr)
        {
            upcs = dicCache[mgr].ListSkillUPC;
            var count = upcs.Count;

            for (int i = 0; i < count; i++)
            {
                //upcs[i].skillEnityObj.getcompent<SkillEnityMgr>.execute();
            }
        }
    }
}
