using DL.Common;
using DL.MCD;
using DL.MCD.Crl;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    public class Bullet_DL : MCDMgr, IResetable
    {
        public ImpactData data;
        private string DetonateEventTag;
        public GameObject muzzleGO;
        public GameObject crMuzzleGO;


        protected override void OnEnable()
        {
            base.OnEnable();
            crMuzzleGO = GameObjectPool.Instance.CreateObject(muzzleGO.name, muzzleGO,transform.position, transform.rotation);
            GameObjectPool.Instance.CollectObject(crMuzzleGO, 1f);
        }

        public override void Init()
        {
            base.Init();
            DetonateEventTag = "BulletDetonateEvent";
    }
        private void OnCollisionEnter(Collision collision)
        {
            //data = new DetonateData();
            data = GameCommonFactory.Instance.CreateObjectByPool<ImpactData>("DL.Common.ImpactData");
            data.Clear();
            data.MgrA = this;
            if (!collision.gameObject.TryGetComponent(out data.MgrB)) return;
            data.coll = collision;
            EventManager.Instance.SendEvent(DetonateEventTag, data);
            GameCommonFactory.Instance.CollectObjectByPool(data);
            data.Clear();
            data = null;
        }

        public void OnReset()
        {
            //if (ListUPC != null || ListUPC.Count >= 0)
            //{
            //    for (int i = 0; i < ListUPC[0].ListUP.Count; i++)
            //    {
            //        ListUPC[0].ListUP[i].ClearValue();
            //    }
            //}

            //for (int i = ListUPC.Count - 1; i >= 0; i--)
            //{
            //    if (i == 0) return;
            //    ListUPC.Remove(ListUPC[i]);
            //}
        }

        public override List<UnitPropertyContainer> GetUPCs(string upcName)
        {
            if (ListUPC.Count <= 0 || ListUPC == null || upcName == null) return null;

            return ListUPC.FindAll(s => s.UPCName == upcName || s.Tags.Contains(upcName));
        }
    }
}
