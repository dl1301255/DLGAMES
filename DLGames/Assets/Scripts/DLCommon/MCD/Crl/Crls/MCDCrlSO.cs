
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public class MCDCrlSO : ScriptableObject
    {
        [SerializeField] protected string id;
        ///// <summary>
        ///// tags[0] = 优先级？
        ///// </summary>
        [SerializeField] protected List<string> tags;

        public List<string> Tags { get => tags; }
        public string ID { get => id; }

        /// <summary>
        /// Crl 初始化 与 mgr无关
        /// </summary>
        public virtual void Init() 
        {
            
        }
        /// <summary>
        /// Crl 重置 与 mgr无关
        /// </summary>
        public virtual void Cloes() { }
        /// <summary>
        /// Mgr 删除 Crl 触发
        /// </summary>
        public virtual void Enable(MCDMgr mgr = null) { }
        public virtual void Disable(MCDMgr mgr = null) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="ps">如不用 =null 避免GC</param>
        public virtual void ExecuteCrl(MCDMgr mgr = null, params object[] ps) { }
        public virtual void ExecuteCrl(MCDMgr mgr = null) { }
        public virtual void EndExecuteCrl(MCDMgr mgr = null) { }
    }
}
