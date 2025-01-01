using DL.Common;
using DL.MCD;
using DL.MCD.Data;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DL
{
    public class Test1 : MonoBehaviour
    {
        public Dictionary<string, string> dicStr = new Dictionary<string, string>();
        public isStr isStr;
        public int n = 1000;

        private void Awake()
        {
            dicStr.Add("1", "dic str");
            isStr = new isStr();
            isStr.str = "is str";
        }

        private void Update()
        {
            //GetDicStr();
            //GetStr();
        }
        [Button]
        public void GetDicStr()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < n; i++)
            {
                print(dicStr["1"]);
            }
            sw.Stop();
            UnityEngine.Debug.Log(string.Format("Stopwatch total: {0} ms", sw.ElapsedMilliseconds));
        }
        [Button]
        public void GetStr()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < n; i++)
            {
                print(isStr.str);
            }
            sw.Stop();
            UnityEngine.Debug.Log(string.Format("Stopwatch total: {0} ms", sw.ElapsedMilliseconds));
        }


        //public List<string> strs;
        //public string str;

        //[Button]
        //public void strsContains() 
        //{
        //    if (strs == null)
        //    {
        //        print("null");
        //    }
        //    else
        //    {
        //        print("null not");
        //    }
        //    if (strs.Contains(str)) 
        //    {
        //        print("true");
        //    }
        //}
        #region
        ////public t1 t1;
        ////public UnitProperty up;
        ////public float TimeScaleValue;
        ////public TimerNode tn1;
        ////public TimerNode tn2;

        ////public float TimerTimeScaale = 1;

        ////public float deltime1;
        ////public float deltime2;
        ////public float deltime3;

        ////public MCDMgr mgr;
        ////public int num;
        ////public List<MCDMgr> mgrs;
        ////public List<GameObject> Objs;
        ////public GameObject Obj;

        ////public GameObject bulletObj;
        ////public GameObject MuzlleObj;
        ////public AudioClip ac;
        ////public Vector3 v3 = Vector3.zero;


        //private void Start()
        //{
        //    //for (int i = 0; i < num; i++)
        //    //{
        //    //    mgrs.Add(Instantiate(mgr));
        //    //    mgrs[i].transform.position = new Vector3(i, 0, 0);
        //    //}
        //    //for (int i = 0; i < num; i++)
        //    //{
        //    //    Objs.Add(Instantiate(Obj));
        //    //    Objs[i].transform.position = new Vector3(i * 10, 0, 0);
        //    //}
        //}
        //[Button]
        //public void Fire()
        //{
        //    Stopwatch sw = Stopwatch.StartNew();
        //    sw.Start();
        //    for (int i = 0; i < num; i++)
        //    {
        //        mgrs[i].ExecuteCrl("FireCrl",bulletObj,MuzlleObj,ac,Vector3.zero);
        //    }
        //    sw.Stop();
        //    UnityEngine.Debug.Log(string.Format("Stopwatch total: {0} ms", sw.ElapsedMilliseconds));
        //}
        //[Button]
        //public void Fire1()
        //{
        //    Stopwatch sw = Stopwatch.StartNew();
        //    sw.Start();
        //    for (int i = 0; i < num; i++)
        //    {
        //        mgrs[i].ExecuteCrl("FireCrl");
        //    }
        //    sw.Stop();
        //    UnityEngine.Debug.Log(string.Format("Stopwatch total: {0} ms", sw.ElapsedMilliseconds));
        //}

        ////[Button]
        ////public void Fire1()
        ////{
        ////    for (int i = 0; i < num; i++)
        ////    {
        ////        mgrs[i].GetCrl("FireCrl").ExecuteCrl(mgrs[i], bulletObj,MuzlleObj,ac,v3);
        ////    }
        ////}


        ////    public string AssetObjLabels;

        ////    [Button]
        ////    public void AssetObj() 
        ////    {
        ////        var obj = AssetsManager.Instance.LoadAsset<GameObject>(AssetObjLabels);
        ////        Instantiate(obj);
        ////    }

        ////    [Button]
        ////    public void timeScale()
        ////    {
        ////        Time.timeScale = TimeScaleValue;
        ////    }
        ////    [Button]
        ////    public void newTimer()
        ////    {
        ////        //tn2 = TimerManager.Instance.doLoop(this, (int)Time.deltaTime, TimerTest, TimeUpdataType.Update);
        ////        tn1 = TimerManager.Instance.doLoop(this, 1f, newTimerTest, TimeUpdataType.Update);
        ////    }

        ////    [Button]
        ////    public void timeScale1()
        ////    {
        ////        print(Time.deltaTime * 1 * 20);
        ////    }


        ////    [Button]
        ////    public void timedeltatime()
        ////    {


        ////    }

        ////    private void floatA()
        ////    {
        ////        return;
        ////    }

        ////    private float testfloatDeltime(float del)
        ////    {
        ////        return del;
        ////    }

        ////    [Button]
        ////    public void timerPause() 
        ////    {
        ////        TimerManager.Instance.MainTimer.OnPause = TimerManager.Instance.MainTimer.OnPause == false ? true : false;
        ////    }


        ////    private void TimerTest()
        ////    {
        ////        //print("Timer__ " + Time.deltaTime);
        ////    }

        ////    private void newTimerTest()
        ////    {
        ////        print("new Timer__ " );

        ////    }

        ////    [Button]
        ////    public void TestToolGetStrArr()
        ////    {
        ////        var v = UPCManager.Instance.UPInstance(up);
        ////        v.WriteValueByString(v.stringVal);
        ////        up = v;
        ////        print(up.GetSubType<UnitPropertyStrArray>().Value[0]);
        ////        up.GetSubType<UnitPropertyStrArray>().Value[0] = "aaaaa";
        ////        print(v.ReadValueByString());

        ////    }
        ////    [Button]
        ////    public void test2()
        ////    {
        ////        var v = AssetsManager.Instance.LoadAsset<AudioClip>("Door 1_1");
        ////        MMSoundManagerSoundPlayEvent.Trigger(v, MMSoundManagerPlayOptions.Default);
        ////        print(AssetsManager.Instance.AssetCachePool.Count);
        ////    }
        ////    [Button]
        ////    public void test()
        ////    {
        ////        var v = GameCommonFactory.Instance.CreateObjectByPool<t1>("DL.t1");
        ////        v.n = 1;
        ////        v.str = "ss";
        ////        t1 = v;
        ////        print(GameCommonFactory.Instance.CacheCreateObjectByPool[t1.GetType().FullName].Count);
        ////    }

        ////    [Button]
        ////    public void test1()
        ////    {
        ////        GameCommonFactory.Instance.CollectObjectByPool(t1);
        ////        print(GameCommonFactory.Instance.CacheCreateObjectByPool[t1.GetType().FullName].Count);
        ////    }

        ////}
        ////[System.Serializable]
        ////public class t1
        ////{
        ////    public int n;
        ////    public string str;
        ////}
        #endregion
    }
    public class isStr
    {
        public string str;
    }
}
