using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    ///<summary>
    ///静态唯一方法
    ///<summary>
    public class Singleton<T> where T : new()
    {
        protected static T _instance;//创建静态实例

        public static T _Instance//为静态赋值
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }

        public virtual T1 GetInstance<T1>() where T1 : T, new()
        {
            if (_instance is T1)
            {
                return (T1)_instance;
            }
            else return SetInstance<T1>();
        }

        public virtual T2 SetInstance<T2>() where T2 : T, new()
        {
            _instance = new T2();
            return (T2)_instance;
        }

        public virtual void Init()
        {
        }

    }
}
