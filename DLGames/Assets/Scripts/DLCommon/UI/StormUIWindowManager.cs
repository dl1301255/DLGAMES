using DL.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class StormUIWindowManager : UIWindowManager
    {
        [InfoBox("GameName + ������������Ӧ����������/����window")]
        public string GameName = "DLGmaes_Storm_";

        [ShowInInspector, Tooltip("����Window")]
        public Dictionary<string, UIWindow> DicWindowCache = new Dictionary<string, UIWindow>();
        public UIWindow CrWindow;

        public override void Init()
        {
            base.Init();
            OnStart();
        }

        public override void OnStart()
        {
            //��ʾ��Ҫ main menu name : DLGmaes_Storm_MainMenuWindow.preafab;
            //�Ȳ�ѯ�Ƿ񻺴� �� �򴴽�
            if (DicWindowCache.TryGetValue("MainmenuWindow", out CrWindow))
            {
                CrWindow.OpenWindow();
            }
            else
            {
                var go = AssetsManager.Instance.LoadAsset<GameObject>("MainmenuWindow");
                CrWindow = GameObjectPool.Instance.CreateObject("MainmenuWindow", go, Vector3.zero, Quaternion.identity).GetComponent<UIWindow>();
                DicWindowCache.Add("MainmenuWindow", CrWindow);
                CrWindow.OpenWindow();
            }
        }


    }
}

