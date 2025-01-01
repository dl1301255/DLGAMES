using DL.InputSys;
using DL.MCD;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class StormPlayerCrl_A : MonoSingleton<StormPlayerCrl_A>
    {
        public List<MCDMgr> ListPlayerCrlObj;
        public List<MCDMgr> ListShipMgr;
        public List<MCDMgr> ListEquipmentMgr;
        public Transform CameraPointTF;
        public Transform EquipmentPointTF;

        public string _shipUpcName;
        public string _shipHvUpName;



        [Button]
        public void shoot() 
        {
            if (ListEquipmentMgr == null) return;
            ListEquipmentMgr[0].ExecuteCrl("FireCrl");
        }

        public override void Init()
        {
            //InitPlayerManager();
            InitEquipmentMgrPoint();
            EventManager.Instance.ListenEvent("PlayerInput_J_Start",onShoot);
        }

        private void onShoot()
        {
            shoot();
        }

        private void InitPlayerManager()
        {
            ListPlayerCrlObj = new List<MCDMgr>();
            ListShipMgr = new List<MCDMgr>();
            ListEquipmentMgr = new List<MCDMgr>();
        }

        private void InitEquipmentMgrPoint()
        {
            if (EquipmentPointTF == null || ListEquipmentMgr.Count <= 0) return;

            for (int i = 0; i < ListEquipmentMgr.Count; i++)
            {
                ListEquipmentMgr[i].transform.SetParent(EquipmentPointTF);
            }

        }

        private void OnEnable()
        {
            EventManager.Instance.ListenEvent<GameObject, Vector2>(InputEventTag._Instance.Move, shipMoveT);
            //¼àÌýÉä»÷
        }

        private void FixedUpdate()
        {
            cameraPointMove();
            equimentsMove();
        }

        private void shipMoveT(GameObject t1, Vector2 t2)
        {
            if (ListPlayerCrlObj == null || ListPlayerCrlObj.Count <= 0) return;

            var v = ListPlayerCrlObj[0].GetComponent<MCDMgr>()?.GetUPC(0)?.GetUP("HV")?.GetSubType<UnitPropertyVector2>();
            if (v == null) return;
            v.V2 = t2;
        }
        //Camera move;
        private void cameraPointMove()
        {
            if (CameraPointTF == null || ListPlayerCrlObj[0] == null) return;
            CameraPointTF.transform.position = ListPlayerCrlObj[0].transform.position;
        }

        private void equimentsMove()
        {
            if (EquipmentPointTF == null || ListPlayerCrlObj[0] == null) return;
            EquipmentPointTF.transform.position = ListPlayerCrlObj[0].transform.position;
        }
    }
}
