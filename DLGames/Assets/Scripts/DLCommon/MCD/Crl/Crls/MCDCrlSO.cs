
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public class MCDCrlSO : ScriptableObject
    {
        [SerializeField] protected string id;
        ///// <summary>
        ///// tags[0] = ���ȼ���
        ///// </summary>
        [SerializeField] protected List<string> tags;

        public List<string> Tags { get => tags; }
        public string ID { get => id; }

        /// <summary>
        /// Crl ��ʼ�� �� mgr�޹�
        /// </summary>
        public virtual void Init() 
        {
            
        }
        /// <summary>
        /// Crl ���� �� mgr�޹�
        /// </summary>
        public virtual void Cloes() { }
        /// <summary>
        /// Mgr ɾ�� Crl ����
        /// </summary>
        public virtual void Enable(MCDMgr mgr = null) { }
        public virtual void Disable(MCDMgr mgr = null) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="ps">�粻�� =null ����GC</param>
        public virtual void ExecuteCrl(MCDMgr mgr = null, params object[] ps) { }
        public virtual void ExecuteCrl(MCDMgr mgr = null) { }
        public virtual void EndExecuteCrl(MCDMgr mgr = null) { }
    }
}
