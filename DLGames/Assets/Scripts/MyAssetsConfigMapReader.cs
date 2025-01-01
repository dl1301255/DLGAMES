using DL.Common;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DL.Common
{
    public class MyAssetsConfigMapReader
    {
        public static MyAssetsConfigMapData MapData;
        public static void InitConfigMap()
        {
            MapData = new MyAssetsConfigMapData();
            string url = null;

#if UNITY_EDITOR || UNITY_STANDALONE//编译器&PC下执行……
            url = "file://" + Application.streamingAssetsPath + "/" + "MyAssetsMap.json";

#elif UNITY_IPHONE//否则 IOS下执行
        			url = "file://" + Application.dataPath + "/Raw/"+"MyAssetsMap.json";

#elif UNITY_ANDROID//否则 安卓下执行……
        			url = "jar:file://" + Application.dataPath + "!/assets/"+"MyAssetsMap.json";
#endif
            if (url == null)
            {
                Debug.Log("没有配置文件");
                return;
            }

            var json = GameTool.ReadFile(url);

            MapData = JsonConvert.DeserializeObject<MyAssetsConfigMapData>(json);

            if (MapData == null)
            {
                Debug.Log("My Assets == null !!! ");
                return;
            }
        }
    }
}
