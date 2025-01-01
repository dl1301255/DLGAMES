using DL.Common;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using UnityEngine.Networking;

namespace DL.MCD
{
    public class UPCManager : MonoSingleton<UPCManager>
    {
        public string flieName;
        [InlineEditor]
        public UPCListSO BaseDataUPCSO;
        [InlineEditor]
        public UPCListSO GameDataUPCSO;

        public Dictionary<string, List<UnitProperty>> dicCacheUP = new Dictionary<string, List<UnitProperty>>();

        //基础数据 和 game数据 SO形势存放ListUPCs
        private string defPath/* = Application.persistentDataPath + "/SpaceDemoSave.json"*/;
        [Searchable, ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "UPName")]
        public List<UnitProperty> ListUP;

        public DLGameCommonTag DLCommonTag;
        public UnitPropertyTag UPTag;
        public MCDTag MCDTag;

        public string DefPath
        {
            get => defPath;
            set
            {
                defPath = Application.persistentDataPath + "/" + flieName + ".json";
            }
        }

        [BoxGroup("当前数据")]
        //public GameDefData DataSO;


        private void OnValidate()
        {
            DefPath = Application.persistentDataPath + "/SpaceDemoSave.json";
        }

        /// <summary>
        /// 通过BaseData listUPC[num] 创建Perfab
        /// </summary>
        [InlineButton("CreaterPerfab")]
        public int CreaterUPCNum;
        public void CreaterPerfab(int n = 0)
        {
            CreaterUPCNum = n == 0 ? CreaterUPCNum : n;
            var v = BaseDataUPCSO.ListUPC[CreaterUPCNum].GetUP("PerfabName");

            if (v == null)
            {
                print("UPC.PerfabName == null_UPC:" + CreaterUPCNum);
                return;
            }

            var obj = AssetsManager.Instance.SimpleloadAsset<GameObject>(v.StringVal);
            if (obj == null)
            {
                print("obj == null");
                return;
            }
            Instantiate(obj);

        }

        /// <summary>
        /// BasetData Get UPC 
        /// </summary>
        /// <param name="name">form UPCName</param>
        /// <returns></returns>
        public virtual UnitPropertyContainer GetBaseDataUPC(string name)
        {
            var upc = BaseDataUPCSO.ListUPC.Find(s => s.UPCName == name);
            if (upc == null) return null;
            var u = new UnitPropertyContainer();
            GameTool.CopyObjInfo(u, upc);
            return u;
        }

        public void CopyInfoUPC(UnitPropertyContainer sourceUpc, UnitPropertyContainer targetUpc)
        {
            for (int i = 0; i < targetUpc.ListUP.Count; i++)
            {
                var v =  sourceUpc.GetUP(targetUpc.ListUP[i].UPName);
                if (v == null || string.IsNullOrEmpty(v.stringVal)) continue;
                targetUpc.ListUP[i].WriteValueByString(v.stringVal);
            }
        }

        public UnitPropertyContainer NewCopyUPC(UnitPropertyContainer upcSource, UnitPropertyContainer upc = null)
        {
            if (upcSource.ListUP == null || upcSource.ListUP.Count <= 0) return null;
            if (upc == null)
            {
                upc = new UnitPropertyContainer();
            }
            foreach (var up in upcSource.ListUP)
            {
                //up存到一个全新的upc中
                var newUp = GameTool.CreateObject(GameTool.GetStr("DL.MCD.Data.UnitProerpty", up.UPTypeName) ) as UnitProperty;
                GameTool.CopyObjInfo(newUp, up);
                upc.AddUP(newUp);
                GameTool.CopyObjInfo(newUp, up);
                newUp.WriteValueByString();
            }

            return upc;
            //GameTool.CopyObjInfo(upc, upcSource);
        }

        [ButtonGroup]
        public void SaveGameData(string path, object Obj)
        {
            string json;

            if (Obj == null)
            {
                json = JsonConvert.SerializeObject(GameDataUPCSO.ListUPC);
            }
            else
            {
                json = JsonConvert.SerializeObject(Obj);
            }

            if (string.IsNullOrEmpty(path))
            {
                path = DefPath;
            }

            GameTool.WriteFile(path, json);
            print("SaveFile" + path);
        }
        [ButtonGroup]
        public List<UnitPropertyContainer> LoadGameData(string path)
        {
            if (path == null)
            {
                path = DefPath;
            }

            var json = GameTool.ReadFile(path);

            var upc = JsonConvert.DeserializeObject<List<UnitPropertyContainer>>(json);
            //DataSO.UPCS = upc;

            print("LoadFile:" + path);
            return upc;
        }

        /// <summary>
        /// 创建1个 未初始化的UP，如果需要init 则必须有 mgr
        /// </summary>
        [InlineButton("CreaterUP")]
        public string CreaterUpName;
        public UnitProperty CreaterUP(string name = null)
        {
            name = name == null ? CreaterUpName : name;

            //UnitProperty up;
            //if (dicCacheUP.ContainsKey(name))
            //{
            //    if (dicCacheUP[name] != null && dicCacheUP[name].Count > 0)
            //    {
            //        up = dicCacheUP[name][0];
            //        return up;
            //    }

            //}


            var str = GameTool.GetStr("DL.MCD.Data.", name);
            var v = GameTool.CreateObject("DL.MCD.Data." + name) as UnitProperty;
#if UNITY_EDITOR
            v.UPName = name;
            ListUP.Add(v);
            print("ListUP.Add  = " + name);
#endif
            return v;
        }

        //private UnitProperty CreaterUpByStr() 
        //{
            
        //}
        public void CollectUP(UnitProperty up) 
        {
            up.ClearValue();
            if (!dicCacheUP.ContainsKey(up.UPName))
            {
                dicCacheUP[up.UPName].Add(up);
            }
            else
            {
                dicCacheUP.Add(up.UPName, new List<UnitProperty>());
                dicCacheUP[up.UPName].Add(up);
            }
        }

        /// <summary>
        /// Assets\Scripts\DLCommon\MCD\Crl\Crls\new\
        /// </summary>
        public string CrlFullAddress;

        [InlineButton("CreaterAssetSO")]
        public string AssetName;
        public ScriptableObject CreaterAssetSO(string AssetName)
        {
            var v = AssetName == null ? this.AssetName : AssetName;
            var so = ScriptableObject.CreateInstance(AssetName);
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(so, CrlFullAddress + AssetName + ".asset");
            AssetDatabase.Refresh();
            print(CrlFullAddress + AssetName);
#endif
            return so;
        }

        /// <summary>
        /// Return UP =  New SubUP
        /// </summary>
        /// <param name="up">up</param>
        /// <param name="initable">是否init</param>
        /// <param name="mgr">mgr</param>
        /// <returns></returns>
        public UnitProperty UPInstance(UnitProperty up, MCDMgr mgr = null)
        {
            if (/*up.UPFullName == "DL.MCD.Data.UnitProperty"*/ string.IsNullOrEmpty(up.UPTypeName))
            {
                up.Init(mgr);
                return up;
            }
            var tempUP = GameCommonFactory.Instance.CreateObjectByPool<UnitProperty>(GameTool.GetStr("DL.MCD.Data.UnitProperty", up.UPTypeName));
            if (tempUP == null) return null;
            GameTool.CopyObjInfo(tempUP, up);
            tempUP.Init(mgr);
            return tempUP;
        }

        //Get base ref 数据
    }
}
