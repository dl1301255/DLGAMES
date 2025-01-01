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
    ///     作用： 初始化 input sender object
    ///            调用 对比 input sender object
    ///            默认：默认 updata 如果datas 没有则 输入 defaultData
    ///                  包含默认值 如 playerBool 和 onUse；
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
