using DL.Common;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DL.MCD.Crl.ObjRigidbodyMove_A_Crl;

namespace DL.MCD
{
    public class ObjRigidbodyMove_Crl : MCDCrlSO
    {
        //ÒÆ¶¯
        [Sirenix.OdinInspector.ShowInInspector]
        private Dictionary<MCDMgr, RgbMoveData> DicCache = new Dictionary<MCDMgr, RgbMoveData>();
        RgbMoveData rmd;
        Rigidbody rgb;
        public float speed;

        public bool Animatorable;
        public bool lockY = true;
        public string animaName = "H";
        public bool Audioable;
        ParamsValue v;
        ParamsValueVector value = new ParamsValueVector();

        public string AudioUP;

        public override void Enable(MCDMgr mgr)
        {
            if (mgr.TryGetComponent(out rgb))
            {
                var v = (mgr as MCDMgr).GetUPC("UnitShip")?.GetUP("HV");
                if (v == null) return;
    
                var d = new RgbMoveData();
                d.rgb = rgb;
                d.hv = v as UnitPropertyVector2;

                d.mgr = mgr as MCDMgr;

                var v1 = (mgr as MCDMgr).GetUPC("UnitShip")?.GetUP("moveSpeed");
                if (v1 != null) rmd.speed = v1 as UnitPropertyFloat;
                
                DicCache.Add(mgr, d);
            }
        }
        public override void Disable(MCDMgr mgr)
        {
            if (DicCache.TryGetValue(mgr, out rmd))
            {
                rmd.Clear();
                DicCache.Remove(mgr);
            }
        }


        
        public  void OnFixedUpdata(MCDMgr mgr)
        {
            if (DicCache.TryGetValue(mgr, out rmd))
            {
                var s = rmd.speed == null ? speed : rmd.speed.Value;
                value.vector2 = rmd.hv.V2;
                EventManager.Instance.SendEvent<MCDMgr, ParamsValueVector>("PlayerMove",rmd.mgr,value);

                rmd.rgb.AddForce(value.vector2.x * s, 0, value.vector2.y * s, ForceMode.Force);
                //if(Animatorable) mgr.GetComponent<Animator>()?.SetFloat(animaName, rmd.hv.V2.x);

                if(lockY)mgr.transform.position = new Vector3(mgr.transform.position.x, 0, mgr.transform.position.z);
            }
        }

        //public class RgbMoveData
        //{
        //    public MCDMgr mgr;
        //    public Rigidbody rgb;
        //    public UnitPropertyVector2 hv;
        //    public UnitPropertyFloat speed;
        //    public void Clear() 
        //    {
        //        mgr = null;
        //        rgb = null;
        //        hv = null;
        //        speed = null;
        //    }
        //}
    }
}
