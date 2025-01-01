using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DL.Common
{
    ///<summary>
    ///静态唯一方法
    ///<summary>
    public class SerializedSingleton<T> : SerializedBehaviour where T : new()//where 约束 要求传进来的T 必须是 MonoSingleton的子类
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
            
            var v = (T1)_Instance;
            if (v == null)
            {
                _instance = new T1();
                v = (T1)_instance;
            }
            return v;
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
