using DL.Common;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    /// <summary>
    /// 直线移动
    /// </summary>
    public class MoveTransform_A_Crl : MCDCrlSO
    {
        public bool Inited = false;
        public MoveTransformCrlData moveData;

        public Vector3 Dir;
        public float DefSpeed;
        public bool forward = true;

        public List<MCDMgr> ListMgr;

        //存放相关事件
        [ShowInInspector]
        public Dictionary<string, MCDCrlSO> DicCrlSO;


        [ShowInInspector]
        private string _speedUpcTag;
        [ShowInInspector]
        private string _speedTag;
        /// <summary>
        /// 方向
        /// </summary>
        [ShowInInspector]
        private string _dirUpcTag;
        [ShowInInspector]
        private string _dirTag;

        public override void Cloes()
        {
            Inited = false;
        }
        public override void Init()
        {
            if (Inited) return;
            DicCrlSO = new Dictionary<string, MCDCrlSO>();
            ListMgr = new List<MCDMgr>();
            moveData = new MoveTransformCrlData();
            
            TimerManager.Instance.doLoop(this, Time.fixedDeltaTime, OnFixedMove);
            Inited = true;
        }
        private void OnDestroy()
        {
            Inited = false;
            Debug.Log("OnDestroy");
        }

        
        private void OnFixedMove()
        {
            for (int i = 0; i < ListMgr.Count; i++)
            {
                moveData.Mgr = ListMgr[i];
                moveData.DirVector = getDir(ListMgr[i], ref moveData.DirVector);
                moveData.Speed = getSpeed(ListMgr[i]);
                if (moveData.Mgr == null) continue;

                //事件
                moveData.Mgr.transform.Translate(moveData.DirVector * moveData.Speed * Time.fixedDeltaTime);
            }
        }

        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            if (!Inited) Init();
            if (!ListMgr.Contains(mgr)) ListMgr.Add(mgr);
        }
        public override void EndExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            if (ListMgr.Contains(mgr)) ListMgr.Remove(mgr);
            
        }
        public override void Disable(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            EndExecuteCrl(mgr);
        }
        private float getSpeed(MCDMgr mgr)
        {
            var s = DefSpeed;

            if (mgr.ListUPC.Count != 0)
            {
                var s1 = mgr.GetUPC(_speedUpcTag)?.GetUP(_speedTag)?.GetSubType<UnitPropertyFloat>()?.Value;

                if (s1 != null)
                {
                    s = s1.Value;
                }
            }

            return s;
        }
        private Vector3 getDir(MCDMgr mgr, ref Vector3 v)
        {
            if (forward)
            {
                v = mgr.transform.forward;
            }
            else
            {
                if (mgr.ListUPC.Count != 0)
                {
                    var v2 = mgr.GetUPC(_dirUpcTag)?.GetUP(_dirTag)?.GetSubType<UnitPropertyVector2>()?.V2;

                    if (v2 != null)
                    {
                        v.x = v2.Value.x;
                        v.y = 0f;
                        v.z = v2.Value.y;
                    }
                }
            }
            return v;
        }
        public override void Enable(MCDMgr mgr = null)
        {
            ExecuteCrl(mgr);
        }
    }
    public class MoveTransformCrlData
    {
        //物体
        public MCDMgr Mgr;
        //方向
        public Vector3 DirVector = Vector3.zero;
        //速度
        public float Speed;

        public UnitPropertyContainer Upc;

        public void Reset()
        {
            Mgr = null;
            DirVector = Vector3.zero;
            Speed = 0;
            if (Upc != null)
            {
                Upc.Clear();
            }
        }

    }
}
