using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DL.Common
{
    public class AssetsManager : MonoSingleton<AssetsManager>
    {
        public Dictionary<string, object> AssetCachePool = new Dictionary<string, object>();
        public MyAssetsConfigMapData mapData;
        object obj = null;

        public override void Init()
        {
            MyAssetsConfigMapReader.InitConfigMap();
            mapData = MyAssetsConfigMapReader.MapData;
        }

        //private void initConfigMap()
        //{
        //    Debug.Log("Init Config Map");
        //    mapData = new MyAssetsConfigMapData();
        //    string url = null;
        //    url = Application.dataPath + "/" + "MyAssetsMap.json";


        //    //string url = "file://" + Application.streamingAssetsPath + "/"+ fileName;//file：// = 本地

        //    //#if UNITY_EDITOR || UNITY_STANDALONE//编译器&PC下执行……
        //    //            url = "file://" + Application.dataPath + "/StreamingAssets/" + "MyAssetsMap.json";
        //    //            //url = Application.dataPath + "/StreamingAssets/" + "MyAssetsMap.json";

        //    //#elif UNITY_IPHONE//否则 IOS下执行
        //    //			url = "file://" + Application.dataPath + "/Raw/"+"MyAssetsMap.json";

        //    //#elif UNITY_ANDROID//否则 安卓下执行……
        //    //			url = "jar:file://" + Application.dataPath + "!/assets/"+"MyAssetsMap.json";
        //    //#endif
        //    if (url == null)
        //    {
        //        Debug.Log("没有配置文件");
        //        return;
        //    }

        //    var json = GameTool.ReadFile(url);
        //    Debug.Log(json);
        //    mapData = JsonConvert.DeserializeObject<MyAssetsConfigMapData>(json);

        //    if (mapData == null)
        //    {
        //        Debug.Log("My Assets == null !!! ");
        //        return;
        //    }
        //}


        //加载缓存
        public T LoadAsset<T>(string key, bool async = false)
        {
            if (string.IsNullOrEmpty(key)) return default;

            object asset;

            if (AssetCachePool.TryGetValue(key, out asset)) return (T)asset;

            asset = LoadMyAssets<T>(key, async);

            if (asset != null) return (T)asset;

            Debug.Log("LoadAsset nol");

            return (T)asset;
        }
        private void onLoad<T>(AsyncOperationHandle<T> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                AssetCachePool.Add(obj.Result.ToString(), obj.Result);
            }
            else
            {
                Debug.LogWarning($"Failed to load asset with key: {obj.Result}");
            }
        }
        //加载缓存
        public bool TryLoadAsset<T>(string key, out T value, bool async = false)
        {
            object asset;

            if (AssetCachePool.TryGetValue(key, out asset))
            {
                value = (T)asset;
                return true;
            }
            else
            {
                value = LoadMyAssets<T>(key, async);
                if (value == null) return false;
                else return true;
            }
        }
        /// <summary>
        /// 同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SimpleloadAsset<T>(string key)
        {
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SimpleloadAsset<T>(AssetReference key)
        {
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        }
        public T SimpleloadAsset<T>(AssetReferenceGameObject key)
        {
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        }
        public void SimpleReleaseAsset(object key)
        {
            Addressables.Release(key);
        }
        public void ReleaseAsset(string key)
        {
            if (AssetCachePool.ContainsKey(key))
            {
                AssetCachePool.Remove(key);
            }
            Addressables.Release(key);
        }

        public T LoadMyAssets<T>(string key, bool async = false)
        {
            if (mapData == null || !mapData.data.ContainsKey(key) || mapData.data[key].ListIsNullorEmpty()) return default;

            //send事件

            if (!async) obj = Addressables.LoadAssetAsync<T>(mapData.data[key][0]).WaitForCompletion();

            else Addressables.LoadAssetAsync<T>(mapData.data[key][0]).Completed += onLoad;

            if (obj == null)
            {
                Debug.Log("key == " + key + "___" + "MapData path == " + mapData.data[key][0]);
                return default(T);
            }

            if (!AssetCachePool.ContainsKey(key))
            {
                AssetCachePool.Add(key, obj);
            }
            else
            {
                AssetCachePool[key] = obj;
            }
            return (T)obj;
        }

    }
}
