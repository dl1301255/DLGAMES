using DL.Common;
using DL.MCD;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    //重置接口，实现逻辑调用顺序更变，创建物体-->OnReset(Awake())-->pool.UseObject.pos --> OnReset()						
    //所有频繁需要创建 销毁的对象 使用；
    /// <summary>
    /// 重置接口 触发顺序
    /// </summary>
    public interface IResetable
    {
        /// <summary>
        /// 启动时 执行方法
        /// </summary>
        void OnReset();
    }
    ///<summary>
    ///游戏物体对象池
    ///MonoSingleton使用方法：<需要创建的类型> 一般创建自己
    ///<summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        public delegate void GameObjectPoolInit(GameObject obj);

        private Dictionary<string, List<GameObject>> cache;
        [ShowInInspector]
        public static GameObject TempPoolObj;

        //定时回收
        public List<CollectObjData> listCollectableObj ;
        public List<CollectObjData> listCollectObjDataCache;
        //初始化对象池
        public override void Init()
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();//new dictionary<>()；
            TempPoolObj = gameObject;
            listCollectableObj = new List<CollectObjData>();
            listCollectObjDataCache = new List<CollectObjData>();
            TimerManager.Instance.doLoop(this, 0, tryCollectableObj);

        }

        private void tryCollectableObj()
        {
            if (listCollectableObj == null || listCollectableObj.Count <= 0) return;
            for (int i = listCollectableObj.Count - 1; i >= 0; i--)
            {
                listCollectableObj[i].time -= Time.fixedDeltaTime;
                if (listCollectableObj[i].time <= 0)
                {
                    CollectObject(listCollectableObj[i].obj);
                    listCollectObjDataCache.Add(listCollectableObj[i]);
                    listCollectableObj.Remove(listCollectableObj[i]);
                }
            }
        }

        /// <summary>
        /// 创建Obj
        /// </summary>
        /// <param name="key">名</param>
        /// <param name="prefab">物体</param>
        /// <param name="pos">位置</param>
        /// <param name="rotate">旋转</param>
        /// <param name="parent">父对象</param>
        /// <param name="delInit">初始化方法</param>
        /// <param name="independent">是否脱离pool</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate, Transform parent = null,
                                        GameObjectPoolInit delInit = null, bool independent = false)
        {
            //查找是否有禁用的gameobject
            GameObject go = FindUsableObject(key);
            //如果 游戏物体 == 空
            if (go == null)
            {
                go = AddObject(key, prefab);
            }
            //使用对象
            //设置Parent
            if (!parent)
            {
                if (TempPoolObj != null)
                {
                    go.transform.SetParent(TempPoolObj.transform);
                }
            }
            else
            {
                go.transform.SetParent(parent);
            }

            if (independent)
            {
                cache[key].Remove(go);
            }

            UesObject(pos, rotate, go, delInit);

            //返回对象
            return go;
        }

        /// <summary>
        /// 获取一个 未启动的Obj，需要手动启用，作用：可在获取后进行设置 并手动启动
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="rotate"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject GetCreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate, Transform parent = null)
        {
            //查找是否有禁用的gameobject
            GameObject go = FindUsableObject(key);
            //如果 游戏物体 == 空
            if (go == null)
            {
                go = AddObject(key, prefab);
            }

            go.transform.position = pos;
            go.transform.rotation = rotate;
            //go.GetComponent<IResetable>()?.OnReset();
            cache[key].Remove(go);

            //设置Parent
            if (!parent)
            {
                if (TempPoolObj != null)
                {
                    go.transform.SetParent(TempPoolObj.transform);
                }
            }
            else
            {
                go.transform.SetParent(parent);
            }

            //返回对象
            return go;
        }

        //使用对象
        private static void UesObject(Vector3 pos, Quaternion rotate, GameObject go, GameObjectPoolInit delInit = null)
        {
            go.GetComponent<IResetable>()?.OnReset();

            go.transform.position = pos;
            go.transform.rotation = rotate;

            if (delInit != null)
            {
                delInit.Invoke(go);
            }


            go.SetActive(true);
        }

        //添加对象
        private GameObject AddObject(string key, GameObject prefab)
        {
            GameObject go = Instantiate(prefab);//创建GameObject prefab
            go.name = key;
            if (!cache.ContainsKey(key))//如果 字典中不包含Key 
            {
                cache.Add(key, new List<GameObject>()); //创建Key 加入value合集 注意Dictionary<>();
            }
            cache[key].Add(go);//prefab加入池中
            return go;
        }

        //查找Key中可用object
        private GameObject FindUsableObject(string key)
        {
            if (cache.ContainsKey(key)) //cache里面包含Key
            {
                return cache[key].Find(g => !g.activeInHierarchy);//查找没有关闭的物体（ActiveInhierarchy = 有效的 层级）
                                                                  //return cache[key].Find(g => !g.activeSelf);//查找没有关闭的物体
            }
            return null;
        }

        //回收对象
        public TimerNode CollectObject(GameObject go, float delay = 0)
        {
            if (delay == 0)
            {
                setActive(go);
                return null;
            }
            else
            {
                //delay *= 1000;
                //var v = TimerManager.Instance.doLoop_repeat<GameObject>(this, (int)delay, setActive, 1, TimeUpdataType.Update, null, go);
                //CollectObjData v = getCollectObjData(go, delay);
                listCollectableObj.Add(getCollectObjData(go, delay));
                return null;
            }
        }//回收对象的对外方法

        private  CollectObjData getCollectObjData(GameObject go, float delay)
        {
            if (listCollectObjDataCache.Count > 0)
            {
                CollectObjData v = listCollectObjDataCache[0];
                listCollectObjDataCache.Remove(v);
                v.Clear();
                v.obj = go;
                v.time = delay;
                return v;
            }
            else
            {
                return new CollectObjData(go, delay);
            }
            
        }

        private void setActive(GameObject go)
        {
            var name = go.name.Replace("(Clone)", "");

            if (cache.ContainsKey(name))
            {
                if (!cache[name].Contains(go))
                {
                    cache[name].Add(go);
                }
            }
            go.SetActive(false);
        }
        public void clear(string key) //获取一个key
        {
            //Destroy(游戏对象)
            //cache[key] ---> List<GameObject>
            //简单删除
            //foreach (var item in cache[key])
            //{
            //	Destroy(cache[key][i]);
            //}
            //简单删除2
            //for (int i = 0; i < cache[key].Count; i++)//遍历cache[key].[i]
            //{
            //	Destroy(cache[key][i]);//[删除]Key下所有的[i]
            //}
            //倒叙删除 - 通常使用
            for (int i = cache[key].Count - 1; i >= 0; i--)
            {
                Destroy(cache[key][i]);
            }
            cache.Remove(key);
        }
        public void clearAll()
        {
            foreach (var key in new List<string>(cache.Keys))//获取cache.keys 放到New
            {
                clear(key);//用clear 删除 会remove（key）造成错误
            }
        }


        private List<ParamsValue> paramsValueCache = new List<ParamsValue>();
        //转换为 Paramsvalue
        //获取Paramvalue
        public ParamsValue GetParamsValue()
        {
            if (paramsValueCache.Count > 0)
            {
                return callPV();
            }
            else
            {
                paramsValueCache.Add(new ParamsValue());
                return callPV();
            }
        }

        private ParamsValue callPV()
        {
            var v = paramsValueCache[0];
            paramsValueCache.Remove(paramsValueCache[0]);

            return v;

        }

        public void CollectParamsValua(ParamsValue v)
        {
            v.Clear();
            paramsValueCache.Add(v);
        }

    }

    [System.Serializable]
    public class CollectObjData
    {
        public GameObject obj;
        public float time;
        public CollectObjData(GameObject go , float t) 
        {
            obj = go;
            time = t;
        }
        public void Clear() 
        {
            obj = null;
            time = 0;
        }
    }
}
