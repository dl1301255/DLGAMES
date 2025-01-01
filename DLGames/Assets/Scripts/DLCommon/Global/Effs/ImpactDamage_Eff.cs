using DL.DLGame;
using DL.MCD;
using DL.MCD.Crl;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    /// <summary>
    /// UnitData Atk 
    /// UP Float
    /// </summary>
    public class ImpactDamage_Eff : IEff
    {
        bool Useabl = false;

        private ImpactDamageEffData data = new ImpactDamageEffData();
        public void Init()
        {
            
        }

        public void Execute(object obj = null)
        {
            if (!Useabl) 
            {
                Useabl = true;
            }

            if (obj == null || obj is ImpactData == false) return;
            var v = obj as ImpactData;
            if (data == null) data = new ImpactDamageEffData();

            data.Clear();

            data.sourceMgr = v.MgrA;
            if (data.sourceMgr == null || data.sourceMgr.ListUPC.ListIsNullorEmpty()) return;

            data.targetMgr = v.MgrB;
            if (data.targetMgr == null || data.targetMgr.ListUPC.ListIsNullorEmpty()) return;

            data.targetHp = data.targetMgr.ListUPC.GetThisList()[0]?.GetUP("HP")?.GetSubType<UnitPropertyFloat_A>();
            if (data.targetHp == null) return;
            data.sourceAtk = data.sourceMgr.GetUP("UnitData","Atk",false)?.GetSubType<UnitPropertyFloat>();

            if (data.sourceAtk == null) return;

            data.coll = v.coll;

            data.atk = data.sourceAtk.Value;

            EventManager.Instance.SendEvent("DamageEff_Event", data);

            data.targetHp.Value -= data.atk;

            if (data.targetHp.Value <= 0)
            {
                EventManager.Instance.SendEvent("Unit_HP_0", v);
            }

            EventManager.Instance.SendEvent("DamageEff_Evented", data);
        }

        public void Execute(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Close(object obj = null)
        {
            throw new System.NotImplementedException();
        }

        public void Close(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public bool OnUse()
        {
            return Useabl;
        }
    }

}
