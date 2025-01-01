using DL.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


///<summary>
///Editor 文件夹 仅在Unity环境运行
///<summary>
public class GenerateConfigMap : Editor
{
    //调试方法 位置F9 ,调试F5 ， 执行游戏 
    //加载Tools菜单选项快捷键 Mnultem 
    [MenuItem("Tools/MyAssets/Generate Config Map File")]
    public static void Generate()//注意需要使用 Menuitem 则需要静态方法 ；
    {
        string[] path = new string[2] { "Assets/MyAssets", "Assets/Scripts/DLCommon/MCD/Crl/SOs" };
        MyAssetsConfigMapData DicMap = new MyAssetsConfigMapData();
        foreach (var item in path)
        {
            generateMap(item, DicMap);
        }

    }

    private static void generateMap(string path, MyAssetsConfigMapData DicMap)
    {
        //创建数组
        string[] prefab = AssetDatabase.FindAssets("t:prefab", new string[] { path }); //AssetDatabase 仅Unity中可用的方法 , (V（t = xxx.perfab）),数组用{}
        string[] png = AssetDatabase.FindAssets("t:Sprite", new string[] { path });
        string[] so = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { path });
        List<string> listMap = new List<string>();


        foreach (var item in prefab)
        {
            listMap.Add(item);
        }
        foreach (var item in png)
        {
            listMap.Add(item);
        }
        foreach (var item in so)
        {
            listMap.Add(item);
        }


        //获取路径
        for (int i = 0; i < listMap.Count; i++)
        {
            listMap[i] = AssetDatabase.GUIDToAssetPath(listMap[i]);//GUID 转换为 Path
                                                                   //获取名称,存入数组 可以用lastindexof 获得/ 区分 但是很麻烦
            string fileName = Path.GetFileNameWithoutExtension(listMap[i]);//没有后缀的[i]

            //listMap[i] = fileName + "," + listMap[i];//获取“名字 = 路径字段”的 string串
            if (DicMap.data.ContainsKey(fileName))
            {
                DicMap.data[fileName].Add(listMap[i]);
            }
            else
            {
                var value = new List<string>();
                DicMap.data.Add(fileName, value);
                DicMap.data[fileName].Add(listMap[i]);
            }

        }

        var json = JsonConvert.SerializeObject(DicMap);
        GameTool.WriteFile(Application.streamingAssetsPath + "/" + "MyAssetsMap.json", json);
        //输出文件
        //File.WriteAllLines(Application.dataPath + "/StreamingAssets/" + "MyAssetsMap.json", json);//文件，写全部行（路径，文件），StreamingAssets特殊文件夹 注意

        AssetDatabase.Refresh();//自动刷新
        Debug.Log(Application.dataPath + " : " + path + " ==> MyAssetsMap.json");
    }
}

