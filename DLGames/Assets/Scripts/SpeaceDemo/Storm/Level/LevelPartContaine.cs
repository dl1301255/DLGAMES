using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.Common;
using System;

namespace DL.Storm
{
    public class LevelPartContaine : MCD.MCDMgr
    {
        public bool Use;
        public bool UpdatePathLP;
        public LevelPart CrLp;
        public List<LevelPart> ListLP = new List<LevelPart>();

        public StormType StormTypeFanctory;

        public virtual void StartLPC()
        {
            InitListLpSort();
            //监听事件
            EventManager.Instance.ListenEvent<GameObject>(DLGameCommonTag._Instance.LPEnd, lpEnd);
            EventManager.Instance.ListenEvent<GameObject>(DLGameCommonTag._Instance.LPStart, lpStart);
            Use = true;
        }

        public virtual void InitListLpSort()
        {
            if (ListLP == null || ListLP.Count <= 0) return;
        }

        protected virtual void lpStart(GameObject t1)
        {
            CrLp = t1.GetComponent<LevelPart>();
        }

        protected virtual void lpEnd(GameObject t1)
        {
            if (!Use) return;
            
            var lp = t1.GetComponentInParent<LevelPart>();
            //LevelManager.Instance.ListPathLP.Remove(lp);
            //LevelManager.Instance.AddPathLP(lp);       
        }

        public virtual void EndLPC()
        {
            //监听事件
            EventManager.Instance.RemoveEvent<GameObject>(DLGameCommonTag._Instance.LPEnd, lpEnd);
    
            EventManager.Instance.RemoveEvent<GameObject>(DLGameCommonTag._Instance.LPStart, lpStart);

            Use = false;
        }
    }

    public abstract class StormType
    {
        //public abstract bool ExeLPC(LevelPartContaine lpc); 
        public abstract List<LevelPart> LPSort(LevelPartContaine lpc );
        public abstract void LPEnd(LevelPartContaine lpc, LevelPart lp);


    }

    public class StormTypeLoop : StormType
    {
        public override void LPEnd(LevelPartContaine lpc, LevelPart lp)
        {
            lpc.ListLP.Remove(lp);
            lpc.ListLP.Add(lp);
            //lp.reset();
        }

        public override List<LevelPart> LPSort(LevelPartContaine lpc)
        {
            if (lpc.ListLP == null || lpc.ListLP.Count <= 0)
            {
                Debug.Log("LPs == null");
                return null;
            }
            return lpc.ListLP;
        }
    }
}
