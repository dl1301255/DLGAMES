using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DL.Common
{
    public  abstract class UIWindow : MonoBehaviour
    {
        public abstract void Init();
        public abstract void OpenWindow();
        public abstract void CloseWindow();
    }

}
