using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class UIWindowManager : DL.Common.MonoSingleton<UIWindowManager>
    {
        public virtual void OnStart()
        {
            print(GetType().Name);
        }
        public virtual void OpenWindow()
        {

        }
        public virtual void CloseWindow()
        {

        }

    }
}
