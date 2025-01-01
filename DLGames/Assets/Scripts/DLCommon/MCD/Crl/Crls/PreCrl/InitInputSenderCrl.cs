using DL.InputSys;
using DL.MCD.Crl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.Common;
using DL.MCD.Data;

namespace DL.MCD.Crl
{
    /// <summary>
    /// Input sender crl 
    ///     ���ã� ��ʼ�� input sender object
    ///            ���� �Ա� input sender object
    ///            Ĭ�ϣ�Ĭ�� updata ���datas û���� ���� defaultData
    ///                  ����Ĭ��ֵ �� playerBool �� onUse��
    ///     
    /// </summary>
    [CreateAssetMenu(fileName = "InitInputSenderCrl", menuName = "ScriptableObject/DLCommon/Common/Crl/InitInputSenderCrl", order = 1)]
    public class InitInputSenderCrl : MCDCrlSO
    {
        public List<object> InputSenders = new List<object>();
        public string PlayerInputMgrObjTag;
        public bool Player;
        public bool FSM;


        public override void Enable(MCDMgr mgr)
        {
            if (Player)
            {
                InputSenders.Add(GameObject.FindGameObjectsWithTag(PlayerInputMgrObjTag)[0].gameObject);
            }
            if (this.FSM)
            {
                InputSenders.Add(mgr.gameObject);
            }
        }

        public override void Disable(MCDMgr mgr)
        {
            if (InputSenders.Count < 0) return;
            
            InputSenders.Clear();
        }
    }
}
