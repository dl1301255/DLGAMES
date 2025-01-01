using DL.Common;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    public class EquipmentEffInitCrl : MCDCrlSO
    {
        //执行时 EquipmentEff  execute
        //先def 后upc 单独用还是一起用
        public string upcName = "UnitData";
        public string upName = "EquipmentInitEffs";
        public List<string> listDefInitEff;
        #region SO版
        //public List<MCDCrlSO> ListDefCrlSos;
        //public override void Enable(MCDMgr mgr = null)
        //{
        //    base.Enable(mgr);
        //    ExecuteCrl(mgr);
        //}
        //public override void Disable(MCDMgr mgr = null)
        //{
        //    base.Disable(mgr);
        //    ClearExecuteCrl(mgr);
        //}

        //private void ClearExecuteCrl(MCDMgr mgr)
        //{
        //    UnitProperty up = mgr.GetUPC(upcName)?.GetUP(upName);
        //    if (up == null) return;
        //    var v = up.GetSubType<UnitPropertyListStr>();

        //    for (int i = 0; i < v.Value.Count; i++)
        //    {
        //        var crl = MCDManager.Instance.GetCrl(v.Value[i]);

        //        if (crl == null)
        //        {
        //            Debug.Log("DL.MCD.Crl." + v.Value[i] + "== null !!!");
        //            continue;
        //        }
        //        else
        //        {
        //            crl.CloseExecuteCrl(mgr);
        //        }
        //    }
        //    for (int i = 0; i < ListDefCrlSos.Count; i++)
        //    {
        //        ListDefCrlSos[i].CloseExecuteCrl(mgr);
        //    }
        //}

        //public override void ExecuteCrl(MCDMgr mgr = null)
        //{
        //    if (mgr == null) return;
        //    InitDefCrls(mgr);
        //    InitUPCCrls(mgr);
        //}

        //private void InitUPCCrls(MCDMgr mgr)
        //{
        //    UnitProperty up = mgr.GetUPC(upcName)?.GetUP(upName);
        //    if (up == null) return;
        //    var v = up.GetSubType<UnitPropertyListStr>();
        //    //Get list crlso;
        //    for (int i = 0; i < v.Value.Count; i++)
        //    {
        //        var crl = MCDManager.Instance.GetCrl(v.Value[i]);
        //        //var crl = GameCommonFactory.Instance.CreateObject<MCDCrlSO>("DL.MCD.Crl." + v.Value[i]);
        //        if (crl == null)
        //        {
        //            Debug.Log("DL.MCD.Crl." + v.Value[i] + "== null !!!");
        //            continue;
        //        }
        //        else
        //        {
        //            crl.ExecuteCrl(mgr);
        //        }
        //    }
        //}


        //private void InitDefCrls(MCDMgr mgr)
        //{
        //    if (ListDefCrlSos == null || ListDefCrlSos.Count <= 0) return;

        //    for (int i = 0; i < ListDefCrlSos.Count; i++)
        //    {
        //        ListDefCrlSos[i].ExecuteCrl(mgr);
        //    }
        //}
        #endregion

        public override void Enable(MCDMgr mgr = null)
        {
            base.Enable(mgr);
            if (mgr == null) return;
            getAndIntEff(mgr);
            initDefEff(mgr);
        }

        public override void Disable(MCDMgr mgr = null)
        {
            base.Disable(mgr);
            if (mgr == null) return;
            initDefEff(mgr,false);
            var v = mgr.GetUP(upcName, upName)?.GetSubType<UnitPropertyListStr>();
            if (v == null) return;
            initEff(mgr, v.Value, false);

        }

        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            if (ps == null) return;

            if ((string)ps[0] == "AddEff")
            {
                addEff();
            }
            else if ((string)ps[0] == "RemoveEff")
            {
                removeEff();
            }
        }

        private void removeEff()
        {
            throw new NotImplementedException();
        }

        private void addEff()
        {
            throw new NotImplementedException();
        }

        void getAndIntEff(MCDMgr mgr)
        {
            var v = mgr.GetUP(upcName, upName)?.GetSubType<UnitPropertyListStr>();
            if (v == null) return;
            initEff(mgr, v.Value);
        }

        private void initEff(MCDMgr mgr, List<string> effs, bool execute = true)
        {
            if (execute)
            {
                if (effs == null && effs.Count <= 0) return;
                
                for (int i = 0; i < effs.Count; i++)
                {
                    /*var eff = */
                    if (string.IsNullOrEmpty(effs[i])) continue;
                    
                    GameCommonFactory.Instance.CreateObject<IEff>(GameTool.GetStr("DL.Common.", effs[i]))?.Execute(mgr);
                }
            }
            else
            {
                for (int i = 0; i < effs.Count; i++)
                {
                    /*var eff = */
                    GameCommonFactory.Instance.CreateObject<IEff>(GameTool.GetStr("DL.Common.", effs[i]))?.Close(mgr);
                }
            }
        }

        private void initDefEff(MCDMgr mgr, bool onExe = true)
        {
            if (listDefInitEff == null || listDefInitEff.Count <= 0) return;

            if (onExe)
            {
                initEff(mgr, listDefInitEff);
            }
            else
            {
                initEff(mgr, listDefInitEff, false);
            }
        }

    }
}
