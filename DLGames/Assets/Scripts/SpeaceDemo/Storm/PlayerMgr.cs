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

        //list �Զ��ͷ� �������� װ��
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
            //�������� ���뵽UPC�б��� ��ִ�з��� �÷�����ִ��ʱeffЧ�������ж� ��ǰִ��Ч�� �����ж�
            //���t1 ���뷽ʽ����0 �� skillUPC inputType = t1
            if (t1 != 0) setSkillUPCInputType(t1);
            ExecuteCrl("ExecuteSkill", ListSkill[id],ListShip[0].transform);
        }

        private void setSkillUPCInputType(int t1)
        {
            
        }

        public void AutoListEquipment() 
        {
            //��ȡEXEskill Crl
            //exeCrl(UPC)
        }
        public void Equip() { }
        public void UnEquip() { }
    }

}
