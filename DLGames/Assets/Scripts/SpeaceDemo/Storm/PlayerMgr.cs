using DL.Common;
using DL.InputSys;
using DL.MCD;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    public class PlayerMgr : MCDMgr
    {
        public List<MCDMgr> ListPlayerCrlMgr;
        public Transform CameraPointTF;

        //list 自动释放 主动技能 装备
        public List<UnitPropertyContainer> ListAutoExeEquipment = new List<UnitPropertyContainer>();
        public List<UnitPropertyContainer> ListSkill = new List<UnitPropertyContainer>();
        public List<MCD.MCDMgr> ListShip = new List<MCD.MCDMgr>();

        public override void Init()
        {
            base.Init();
            EventManager.Instance.ListenEvent<int>("PlayerInput_J_Start", GetOnSkill_01);
            //EventManager.Instance.ListenEvent("PlayerInput_K_Start", OnSkill_02);
            //EventManager.Instance.ListenEvent("PlayerInput_L_Start", OnSkill_03);

            for (int i = 0; i < ListSkill.Count; i++)
            {
                ListSkill[i].Init(this);
            }
        }

        private void GetOnSkill_01(int t1)
        {

            ExeSkill(0,t1);
        }

        protected override void Enable()
        {
            EventManager.Instance.ListenEvent<GameObject, Vector2>(InputEventTag._Instance.Move, shipMoveT);
        }

        private void FixedUpdate()
        {
            cameraPointMove();
        }

        private void shipMoveT(GameObject t1, Vector2 t2)
        {
            if (ListPlayerCrlMgr == null || ListPlayerCrlMgr.Count <= 0) return;

            var v = ListPlayerCrlMgr[0].GetComponent<MCDMgr>()?.GetUPC(0)?.GetUP("HV")?.GetSubType<UnitPropertyVector2>();
            if (v == null) return;
            v.V2 = t2;
        }
        //Camera move;
        private void cameraPointMove()
        {
            if (CameraPointTF == null || ListPlayerCrlMgr[0] == null) return;
            CameraPointTF.transform.position = ListPlayerCrlMgr[0].transform.position;
        }

        private void OnSkill_01(object[] args)
        {
            ExeSkill(0);
        }
        private void OnSkill_02(object[] args)
        {
            ExeSkill(1);
        }
        private void OnSkill_03(object[] args)
        {
            ExeSkill(2);
        }

        [Button]
        public void ExeSkill(int id = 0, int t1 = 0) 
        {
            if (ListSkill[id] == null) return;
            //输入类型 加入到UPC中保存 并执行方法 让方法在执行时eff效果进行判断 当前执行效果 进行行动
            //如果t1 输入方式不是0 则 skillUPC inputType = t1
            if (t1 != 0) setSkillUPCInputType(t1);
            ExecuteCrl("ExecuteSkill", ListSkill[id],ListShip[0].transform);
        }

        private void setSkillUPCInputType(int t1)
        {
            
        }

        public void AutoListEquipment() 
        {
            //获取EXEskill Crl
            //exeCrl(UPC)
        }
        public void Equip() { }
        public void UnEquip() { }
    }

}
