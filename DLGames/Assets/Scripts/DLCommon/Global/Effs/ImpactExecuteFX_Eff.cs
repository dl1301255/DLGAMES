using DL.DLGame;
using DL.MCD;
using DL.MCD.Crl;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class ImpactExecuteFX_Eff : IEff
    {
        bool useabl = false;
        private string[] detonateFXPath;
        GameObject FXPrefab;
        float duration;
        UnitProperty fxUP;
        string str;

        public void Init()
        {
            detonateFXPath = new string[2] { "UnitData", "ImpactFXPrefab" };
            duration = 0.5f;
        }
        public void Execute(object obj = null)
        {
            if (!useabl)
            {
                Init();
                useabl = true;
            }

            if (obj == null || obj is ImpactData == false) return;
            var v = obj as ImpactData;

            str = null;
            fxUP = null;

            if (v.MgrA.TryGetUP(detonateFXPath[0], detonateFXPath[1], out fxUP) && !string.IsNullOrEmpty(fxUP.stringVal)) 
            {
                str = fxUP.stringVal;
            }
            else
            {
                str = GameTool.GetStr(v.MgrA.gameObject.name.Replace("(Clone)", ""));
            }
            if (string.IsNullOrEmpty(str)) return;
            
            if (!AssetsManager. Instance.TryLoadAsset(GameTool.GetStr(str, "_Impact"), out FXPrefab)) return;
            var g = GameObjectPool.Instance.CreateObject(FXPrefab.name, FXPrefab, v.coll.GetContact(0).point, Quaternion.LookRotation(v.coll.GetContact(0).normal * -1f));
            GameObjectPool.Instance.CollectObject(g, duration);
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
