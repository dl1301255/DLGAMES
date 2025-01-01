using DL.Common;
using DL.MCD;
using DL.MCD.Data;
using MoreMountains;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace DL
{
    public class Test : MonoBehaviour
    {
        public int[] arrInt = new int[] { 1, 2, 3 };


        private void Start()
        {
            eventTest01Sended();

        }

        [Button]

        public void EventTest01()
        {
            //ParamsValue v = GameObjectPool.Instance.GetParamsValue();
            //v.FloatValue = arrInt[1];
            EventManager.Instance.SendEvent<float, int>("testArrInt", arrInt[1], 0);
            //GameObjectPool.Instance.CollectParamsValua(v);


        }
        [Button]

        public void EventTest02()
        {

            EventManager.Instance.RemoveEvent<float, int>("testArrInt", testEventLis03);

            //EventManager.Instance.RemoveDelegate("testArrInt", testEventLis04,args);

        }
        private void eventTest01Sended()
        {
            //EventManager.Instance.ListenEvent<ParamsValue>("testArrInt", testEventLis02, 6);
            //EventManager.Instance.ListenEvent<ParamsValue>("testArrInt", testEventLis01, 0);

            EventManager.Instance.ListenEvent<float, int>("testArrInt", testEventLis04, 6);
            EventManager.Instance.ListenEvent<float, int>("testArrInt", testEventLis03, 0);
        }

        private void testEventLis03(float t1, int t2)
        {
            print("testEvent03___" + t1 + 3);
        }

        private void testEventLis04(float t1, int t2)
        {
            print("testEvent04___" + t1 + 4);
        }

        private void testEventLis02(ParamsValue t1)
        {
            print("testEvent02___" + t1.FloatValue + 1);
        }

        private void testEventLis01(ParamsValue t1)
        {
            print("testEvent01___" + t1.FloatValue);
        }
    }
}
