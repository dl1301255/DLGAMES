using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public class MCDMgr1 : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        protected virtual void OnEnable()
        {
            Enable();
        }

        protected virtual void OnDisable()
        {
            Disable();
        }

        public virtual void Init() { }
        protected virtual void Enable() { }
        protected virtual void Disable() { }

        public virtual void ExecuteCrl() { }
        public virtual void ExecuteCrl(params object[] args) { }

        public virtual void ExecuteCrl(int index = 0) { }
        public virtual void ExecuteCrl(int index, params object[] args) { }
        public virtual void ExecuteCrl(string tag) { }
        public virtual void ExecuteCrl(string tag, params object[] args) { }
    }
}
