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

#if UNITY_EDITOR || UNITY_STANDALONE//������&PC��ִ�С���
            url = "file://" + Application.streamingAssetsPath + "/" + "MyAssetsMap.json";

#elif UNITY_IPHONE//���� IOS��ִ��
        			url = "file://" + Application.dataPath + "/Raw/"+"MyAssetsMap.json";

#elif UNITY_ANDROID//���� ��׿��ִ�С���
        			url = "jar:file://" + Application.dataPath + "!/assets/"+"MyAssetsMap.json";
#endif
            if (url == null)
            {
                Debug.Log("û�������ļ�");
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
