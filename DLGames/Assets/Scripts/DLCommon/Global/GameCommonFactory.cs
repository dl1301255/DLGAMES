using DL.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class GameCommonFactory : MonoSingleton<GameCommonFactory>,ICache
    {
        public Dictionary<string, object> DicMethodCache;
        public Dictionary<string, string[]> DicStrAryCache;
        //生成RelfID 8位 0-9 A-Z 
        public int crID;

        public override void Init()
        {
            DicMethodCache = new Dictionary<string, object>();
            DicStrAryCache = new Dictionary<string, string[]>();
        }

        //缓存创建指定T型 数据
        public T CreateObject<T>(string className) where T : class
        {
            if (DicMethodCache.ContainsKey(className))
            {
                return DicMethodCache[className] as T;
            }
            else
            {
                Type type = Type.GetType(className);//反射1：获取类型：获取反射类型（字符串）
                object instance = Activator.CreateInstance(type);//反射2：创建对象：激活.创建实例化（type）
                DicMethodCache.Add(className, instance);
                return instance as T;
            }
        }

        #region Create Object Pool

        public Dictionary<string, List<object>> CacheCreateObjectByPool = new Dictionary<string, List<object>>();
        /// <summary>
        /// 基于Pool 创建Obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        public T CreateObjectByPool<T>(string className) where T : class
        {
            if (CacheCreateObjectByPool.ContainsKey(className))
            {
                if (CacheCreateObjectByPool[className].Count > 0) return getCacheType<T>(className);
                else
                {
                    var instance = CreateObject(className);
                    if (instance == null)
                    {
                        Debug.Log(className + "__CreaterObject = null");
                        return null;
                    }
                    CacheCreateObjectByPool[className].Add(instance);
                    return getCacheType<T>(className);
                }
            }
            else
            {
                var instance = CreateObject(className);
                if (instance == null)
                {
                    Debug.Log(className + " == null ");
                    return null;
                }
                var list = new List<object>();
                list.Add(instance);
                CacheCreateObjectByPool.Add(className, list);
                return getCacheType<T>(className);
            }
        }
        private T getCacheType<T>(string className) where T : class
        {
            var v = CacheCreateObjectByPool[className][0] as T;
            CacheCreateObjectByPool[className].Remove(v);
            return v;
        }
        public void CollectObjectByPool(object obj)
        {
            string name = obj.GetType().FullName;

            if (CacheCreateObjectByPool.ContainsKey(name))
            {
                CacheCreateObjectByPool[name].Add(obj);
            }
            else
            {
                var list = new List<object>();
                list.Add(obj);
                CacheCreateObjectByPool.Add(name, list);
            }
        }
        #endregion

        #region Create Method
        public Dictionary<string, object> CacheCreateMethod = new Dictionary<string, object>();

        /// <summary>
        /// 创建方法 并缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        public T CreateMethod<T>(string className, bool cache = true) where T : class
        {
            if (cache)
            {
                if (CacheCreateMethod.ContainsKey(className))
                {
                    return CacheCreateMethod[className] as T;
                }
                else
                {
                    object instance = CreateObject(className);
                    //Type type = Type.GetType(className);//反射1：获取类型：获取反射类型（字符串）
                    //object instance = Activator.CreateInstance(type);//反射2：创建对象：激活.创建实例化（type）
                    CacheCreateMethod.Add(className, instance);
                    return instance as T;
                }
            }
            else
            {
                object instance = CreateObject(className);
                return instance as T;
            }
        }

        /// <summary>
        /// 快速创建obj 并返回obj
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public object CreateObject(string typeName)
        {
            object obj = null;

            try
            {
                Type objType = Type.GetType(typeName, true);
                obj = Activator.CreateInstance(objType);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return obj;
        }
        #endregion

        public string[] GetStrAry(string str, char slipt = ',') 
        {
            string[] strs;

            if (!DicStrAryCache.TryGetValue(str,out strs))
            {
                strs = GameTool.GetStringToArray(str, slipt);
                DicStrAryCache.Add(str, strs);
            }
            return strs;
        }

        public void Clear() 
        {
            DicMethodCache.Clear();
        }

        public void CacheClear()
        {
            DicMethodCache.Clear();
            DicStrAryCache.Clear();
            CacheCreateObjectByPool.Clear();
            CacheCreateMethod.Clear();
        }

        public string GetID() 
        {
            crID++;
            return GameTool.GetStr("_", crID.ToString());
        }
    }
}
