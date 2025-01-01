using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DL.InputSys
{
    /// <summary>
    ///用于发射事件
    /// 变量：发射者 sender 、收信人 recipient、值value
    /// </summary>
    public abstract class InputMgr : MonoBehaviour
    {
        public InputActionAsset Asset;

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Enabled();
            Asset.Enable();
        }

        private void OnDisable()
        {
            Disable();
            Asset.Disable();
        }

        /// <summary>
        /// 为ASS赋值
        /// </summary>
        protected abstract void Init();

        protected abstract void Disable();

        protected abstract void Enabled();
    }
}
