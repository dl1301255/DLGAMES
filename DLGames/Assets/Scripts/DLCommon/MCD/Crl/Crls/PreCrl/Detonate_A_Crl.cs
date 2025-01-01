using DL.Common;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DL.MCD.Crl
{
    /// <summary>
    /// �ӵ���ըЧ�� 
    /// ���Ȳ��� UPC��Effs ��˳�� ����DefEffs
    /// </summary>
    public class Detonate_A_Crl : MCDCrlSO
    {
        public List<string> DefEffs;                                                    //���� ��Ч ���� �¼�
        public StringBuilder sb = new StringBuilder();
        private List<string> effs;
        //���Mgreff
        private string _effStr = "Eff";
        public List<MCDCrlSO> ListCrlSO;

        /// <summary>
        /// ԭ������ ��Ŀ��
        /// </summary>
        /// <param name="mgr"></param>
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            sb.Clear();
            effs = mgr.GetUPC("UnitData")?.GetUP("BDEffs")?.GetSubType<UnitPropertyListStr>().Value;

            if (effs == null || effs.Count <= 0) effs = DefEffs;

            //��mcdcommonMnager�л�ȡ effCrl
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
        /// �����¼����� 
        /// </summary>
        /// <param name="mgr">source ������</param>
        /// <param name="ps">target ������Ŀ��</param>

        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            sb.Clear();
            effs = mgr.GetUPC("UnitData")?.GetUP("BDEffs")?.GetSubType<UnitPropertyListStr>().Value;

            if (effs == null || effs.Count <= 0) effs = DefEffs;

            //��mcdcommonMnager�л�ȡ effCrl
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
