using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    ///<summary>
    ///
    ///<summary>
    public class MyTest_EventManager : MonoBehaviour
    {
        public int[] arrInt = new int[] { 1, 2, 3 };


        private void Start()
        {
            eventTest01Sended();
        }

        [Button]
        public void EventTest01()
        {
            EventManager.Instance.SendEvent<float, int>("testArrInt", arrInt[1], 0);
        }

        [Button]
        public void EventTest02()
        {
            EventManager.Instance.RemoveEvent<float, int>("testArrInt", testEventLis03);
        }

        private void eventTest01Sended()
        {
            EventManager.Instance.ListenEvent<float, int>("testArrInt", testEventLis04, 6);
            EventManager.Instance.ListenEvent<float, int>("testArrInt", testEventLis03, 0);
        }

        private void testEventLis03(float t1, int t2)
        {
            print("testEvent03___" + t1 + 1);
        }

        private void testEventLis04(float t1, int t2)
        {
            print("testEvent04___" + t1 + 1);
        }
    }
}

