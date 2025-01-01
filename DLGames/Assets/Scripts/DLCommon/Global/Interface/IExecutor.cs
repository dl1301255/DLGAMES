using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public interface IExecutor 
    {
        public bool OnUse();
        public void Init();
        public void Execute(object obj = null);
        public void Execute(params object[] obj);

        public void Close(object obj = null);
        public void Close(params object[] obj);
    }
}
