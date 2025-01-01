using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using Sirenix.OdinInspector;
using System.Text;

namespace DL.MCD.Crl
{
    public class FxEvent_Crl : MCDCrlSO
    {
        //public GameObject DefFX;
        GameObject ImpactObj;

        /// <summary>
        /// 持续时间
        /// </summary>
        //[ShowInInspector]
        public float du;


        Collision coll;
        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            coll = ps[1] as Collision;
            if (mgr == null || coll == null) return;

            if (!AssetsManager.Instance.TryLoadAsset(GameTool.GetStr(mgr.gameObject.name.Replace("(Clone)", ""), "_Impact"), out ImpactObj)) return;
            var g = GameObjectPool.Instance.CreateObject(ImpactObj.name, ImpactObj, coll.GetContact(0).point, Quaternion.LookRotation(coll.GetContact(0).normal * -1f));
            GameObjectPool.Instance.CollectObject(g, du);
            //if (UpcPath != null && UpcPath.Length > 0 )
            //{
            //    //获取子弹名字和效果
            //}
            //else if(DefFX != null)
            //{
            //    var g = GameObjectPool.Instance.CreateObject(DefFX.name, DefFX, coll.GetContact(0).point, Quaternion.LookRotation(coll.GetContact(0).normal * -1f));
            //    if (g!=null) GameObjectPool.Instance.CollectObject(g, du);
            //}



        }
    }
}
