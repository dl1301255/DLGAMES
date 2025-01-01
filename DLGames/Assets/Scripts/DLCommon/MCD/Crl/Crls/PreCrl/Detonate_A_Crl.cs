using DL.Common;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DL.MCD.Crl
{
    /// <summary>
    /// 子弹爆炸效果 
    /// 优先播放 UPC中Effs 按顺序 播放DefEffs
    /// </summary>
    public class Detonate_A_Crl : MCDCrlSO
    {
        public List<string> DefEffs;                                                    //声音 特效 销毁 事件
        public StringBuilder sb = new StringBuilder();
        private List<string> effs;
        //获得Mgreff
        private string _effStr = "Eff";
        public List<MCDCrlSO> ListCrlSO;

        /// <summary>
        /// 原地引爆 无目标
        /// </summary>
        /// <param name="mgr"></param>
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            sb.Clear();
            effs = mgr.GetUPC("UnitData")?.GetUP("BDEffs")?.GetSubType<UnitPropertyListStr>().Value;

            if (effs == null || effs.Count <= 0) effs = DefEffs;

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

        /// <summary>
        /// 引爆事件触发 
        /// </summary>
        /// <param name="mgr">source 引爆者</param>
        /// <param name="ps">target 被引爆目标</param>

        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            sb.Clear();
            effs = mgr.GetUPC("UnitData")?.GetUP("BDEffs")?.GetSubType<UnitPropertyListStr>().Value;

            if (effs == null || effs.Count <= 0) effs = DefEffs;

            //从mcdcommonMnager中获取 effCrl
            for (int i = 0; i < effs.Count; i++)
            {
                sb.Append("DL.Common.").Append(effs[i]).Append(_effStr);
                GameCommonFactory.Instance.CreateMethod<IEff>(sb.ToString())?.Execute(mgr, ps);
                sb.Clear();
            }

            for (int i = 0; i < ListCrlSO.Count; i++)
            {
                ListCrlSO[i].ExecuteCrl(mgr, ps);
            }
        }

    }
}
