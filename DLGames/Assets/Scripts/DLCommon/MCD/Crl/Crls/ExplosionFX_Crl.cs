using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using Sirenix.OdinInspector;

namespace DL.MCD.Crl
{
    public class ExplosionFX_Crl : MCDCrlSO
    {
        public bool useable = false;
        public string[] detonateFXPath;
        GameObject FXPrefab;
        public float duration;
        UnitProperty fxUP;
        string str;

        [Button]
        public override void Init()
        {
            detonateFXPath = new string[2] { "UnitData", "ExplosionFXPrefab" };
            duration = 2f;
            useable = true;
        }

        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            Execute(mgr);
        }


        public void Execute(object obj = null)
        {
            if (!useable) Init();

            if (obj == null || obj is MCDMgr == false) return;
            var v = obj as MCDMgr;

            str = null;
            fxUP = null;

            if (v.TryGetUP(detonateFXPath[0], detonateFXPath[1], out fxUP) && !string.IsNullOrEmpty(fxUP.stringVal))
            {
                str = fxUP.stringVal;
            }
            else
            {
                str = GameTool.GetStr(v.gameObject.name);
            }
            if (string.IsNullOrEmpty(str)) return;

            if (!AssetsManager.Instance.TryLoadAsset(GameTool.GetStr(str, "_Explosion"), out FXPrefab)) return;

            v.gameObject.SetActive(false);
            var g = GameObjectPool.Instance.CreateObject(FXPrefab.name, FXPrefab, v.transform.position, v.transform.rotation);
            GameObjectPool.Instance.CollectObject(g, duration);
        }
        public void Execute(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        public override void Cloes()
        {
            base.Cloes();
            if (useable)
            {
                useable = false;
            } 
        }


        public bool OnUse()
        {
            return useable;
        }

    }
}
