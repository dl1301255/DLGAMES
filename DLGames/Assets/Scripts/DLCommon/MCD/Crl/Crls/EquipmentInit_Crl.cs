using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using System.Text;

namespace DL.MCD.Crl
{
    public class EquipmentInit_Crl : MCDCrlSO
    {
        public StringBuilder sb = new StringBuilder();
        //读取目标的 InitEffs
        private List<string> effs;
        private string _effStr = "Eff";
        public List<string> DefEffs;
        //默认自带的效果
        public List<MCDCrlSO> ListCrlSO;


        /// <summary>
        /// 初始化equiment 包含触发 监听事件 默认无 子资源
        /// </summary>
        /// <param name="mgr"></param>
        public override void Enable(MCDMgr mgr = null)
        {
            base.Enable(mgr);
            ExecuteCrl(mgr);

        }
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            sb.Clear();
            effs = mgr.GetUPC("UnitData")?.GetUP("BDEffs")?.GetSubType<UnitPropertyListStr>().Value;

            if (effs == null) effs = DefEffs;

            //从mcdcommonMnager中获取 effCrl
            for (int i = 0; i < effs.Count; i++)
            {
                sb.Append("DL.Common.").Append(effs[i]).Append(_effStr);
                GameCommonFactory.Instance.CreateMethod<IEff>(sb.ToString())?.Execute(mgr);
                sb.Clear();
            }

            for (int i = 0; i < ListCrlSO.Count; i++)
            {
                ListCrlSO[i].ExecuteCrl(mgr);
            }
        }
    }


}
