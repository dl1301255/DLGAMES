using DL.Common;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Crl
{
    /// <summary>
    /// 监听damage事件 播放atkNum数值
    /// damageData
    /// </summary>
    public class AtkNumCrl : MCDCrlSO
    {
        public bool inited = false;
        public string EventName = "DamageEff_Evented";
        public string atkNumPerfabName = "AtkNum";
        public float atkNumDelTime = 0.3f;
        public GameObject AtkNumObj;
        ImpactDamageEffData data;
        GameObject go;

        public override void Init()
        {
            if (inited) return;
            
            //监听事件 并释放 exe
            EventManager.Instance.ListenEvent<ImpactDamageEffData>(EventName, CreaterAtkNum);
            AtkNumObj=  AssetsManager.Instance.LoadAsset<GameObject>(atkNumPerfabName);

            inited = true;
        }


        public override void Disable(MCDMgr mgr = null)
        {
            EventManager.Instance.RemoveEvent<ImpactDamageEffData>(EventName, CreaterAtkNum);
            inited = false;
        }

        public override void Cloes()
        {
            EventManager.Instance.RemoveEvent<ImpactDamageEffData>(EventName, CreaterAtkNum);
            inited = false;
        }

        public override void EndExecuteCrl(MCDMgr mgr = null)
        {
            EventManager.Instance.RemoveEvent<ImpactDamageEffData>(EventName, CreaterAtkNum);
        }

        private void CreaterAtkNum(ImpactDamageEffData arg)
        {
            data = arg;

            //CreaaterAtkNum(data.targetMgr.transform.position, data.targetMgr.transform.rotation, atkNumDelTime);
            //CreaaterAtkNum(data.coll.contacts[0].point, data.coll.transform.rotation, atkNumDelTime);
            Vector3 v;
            if (data.coll != null)
            {
                v = data.coll.GetContact(0).point;
            }
            else if (data.targetMgr != null)
            {
                v = data.targetMgr.transform.position;
            }
            else
            {
                return;
            }

            CreaaterAtkNum(v, data.coll.transform.rotation, atkNumDelTime);

            //if (go != null)
            //{
            //    go.transform.rotation = Quaternion.LookRotation(CameraGO.Instance.transform.forward);
            //    go.GetComponent<TextMesh>().text = data.atk.ToString();
            //    Debug.Log(data.atk.ToString());
            //}
            go = null;
            data = null;
        }

        private GameObject CreaaterAtkNum(Vector3 pos, Quaternion rot, float collectDelay = 0.6f)
        {
            if (!AtkNumObj) return null;

            go = GameObjectPool.Instance.CreateObject(AtkNumObj.name, AtkNumObj, pos, rot);
            if (go == null) return null;
            GameObjectPool.Instance.CollectObject(go, collectDelay);
            go.transform.rotation = Quaternion.LookRotation(CameraGO.Instance.transform.forward);
            go.GetComponent<TextMesh>().text = data.atk.ToString();

            return go;
        }
    }
}
