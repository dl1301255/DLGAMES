using DL.Common;
using DL.InputSys;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL
{
    public class UnitTest : MonoBehaviour
    {
        public GameObject obj1;
        public GameObject obj2;

        [Button]
        public void Test1()
        {
            test3(obj1);
        }
        [Button]
        public void Test2()
        {
            test4(obj2);
        }
        public void test4(params object[] args) 
        {

        }

        public void test3(GameObject obj)
        {
            TimerManager.Instance.doLoop_express<GameObject>(this, 1000, close,null,gameObject);


        }


        private void close(GameObject param1)
        {
            print(param1.name);
        }
    }
}
