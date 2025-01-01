using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DL.InputSys
{
    /// <summary>
    ///���ڷ����¼�
    /// ������������ sender �������� recipient��ֵvalue
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
        /// ΪASS��ֵ
        /// </summary>
        protected abstract void Init();

        protected abstract void Disable();

        protected abstract void Enabled();
    }
}
