using DL.DLGame;
using DL.MCD;
using DL.MCD.Crl;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class CollectPoolObj_0_Eff : IEff
    {
        bool useabl = false;

        public void Init()
        {
            useabl = true;
        }
        public void Execute(object obj = null)
        {
            if (!useabl) Init();

            if (obj == null || obj is ImpactData == false) return;
            var v = obj as ImpactData;
            GameObjectPool.Instance.CollectObject(v.MgrA.gameObject);
        }

        public void Execute(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            useabl = false;
        }

        public void Close(object obj = null)
        {
            throw new System.NotImplementedException();
        }

        public void Close(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public bool OnUse()
        {
            return useabl;
        }


    }

}
