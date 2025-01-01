using DL.DLGame;
using DL.MCD;
using DL.MCD.Crl;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class Damage_Eff : IEff
    {
        private ImpactDamageEffData data = new ImpactDamageEffData();
        public void Execute(object obj = null)
        {
           


        }

        public void Execute(params object[] args)
        {
            if (args == null ) return;
            if (data == null) data = new ImpactDamageEffData();

            data.Clear();

            data.sourceMgr = args[0] as MCDMgr;
            if (data.sourceMgr == null) return;

            data.targetMgr = (args[1] as object[])[0] as MCDMgr;
            if (data.targetMgr == null) return;

            data.targetHp = data.targetMgr.ListUPC.GetThisList()[0]?.GetUP("HP")?.GetSubType<UnitPropertyFloat_A>();
            if (data.targetHp == null) return;

            data.sourceAtk = data.sourceMgr.ListUPC.GetThisList()[0]?.GetUP("Atk")?.GetSubType<UnitPropertyFloat>();
            if (data.sourceAtk == null) return;

            data.coll = (args[1] as object[])[1] as Collision;

            data.atk = data.sourceAtk.Value;

            EventManager.Instance.SendEvent("DamageEff_Event", data);
            data.targetHp.Value -= data.atk;

            if (data.targetMgr.TryGetComponent(out ObjFlickerEff flick))
            {
                flick.Execute();
            }

            EventManager.Instance.SendEvent("DamageEff_Evented", data);

            //EventManager.Instance.SendEvent(DLGameCommonTag._Instance.EndDamageEvent, mgr);

        }

        public void Execute()
        {
            return;
        }

        public void Init()
        {
            data = new ImpactDamageEffData();
        }
        public void Close()
        {
            
        }
        public bool OnUse()
        {
            return default;
        }

        public void Close(object obj = null)
        {
            throw new System.NotImplementedException();
        }

        public void Close(params object[] obj)
        {
            throw new System.NotImplementedException();
        }
    }


}
