using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


///<summary>
///Editor 文件夹 仅在Unity环境运行
///<summary>
public class GenerateResConfig : Editor
{
    //调试方法 位置F9 ,调试F5 ， 执行游戏 
    //加载Tools菜单选项快捷键 Mnultem 
    [MenuItem("Tools/Resources/Generate ResConfig File")]
    public static void Generate()//注意需要使用 Menuitem 则需要静态方法 ；
    {
        //创建数组
        string[] resPrefab = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" }); //AssetDatabase 仅Unity中可用的方法 , (V（t = xxx.perfab）),数组用{}
        string[] resPng = AssetDatabase.FindAssets("t:Sprite", new string[] { "Assets/Resources" });
        string[] resFiles = new string[resPrefab.Length + resPng.Length];

        resPrefab.CopyTo(resFiles, 0);
        resPng.CopyTo(resFiles, resPrefab.Length);

        //获取路径
        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);//GUID 转换为 Path
                                                                     //获取名称,存入数组 可以用lastindexof 获得/ 区分 但是很麻烦
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);//没有后缀的[i]
            string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty)
                                .Replace(".prefab", string.Empty)
                                .Replace(".png", string.Empty);//（Resource不显示） 路径 （.xxx 不显示）string串
            resFiles[i] = fileName + "=" + filePath;//获取“名字 = 路径字段”的 string串
        }

        //输出文件
        File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", resFiles);//文件，写全部行（路径，文件），StreamingAssets特殊文件夹 注意
        AssetDatabase.Refresh();//自动刷新
    }
}

