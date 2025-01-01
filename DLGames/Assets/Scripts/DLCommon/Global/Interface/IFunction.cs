using DL.MCD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public interface IFunction : IExecutor
    {
        public void Init(MCDMgr mgr = null);
        public void Enable(MCDMgr mgr = null);
        public void Disable(MCDMgr mgr = null);
        public void OnUpdate(MCDMgr mgr = null);
        public void OnFixedUpdata(MCDMgr mgr = null);
    }
}
