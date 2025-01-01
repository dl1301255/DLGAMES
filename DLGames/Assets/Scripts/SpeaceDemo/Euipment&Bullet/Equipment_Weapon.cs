using DL.Common;
using DL.MCD;
using DL.MCD.Data;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    /// <summary>
    /// �洢���� ����CD Ԥ���ӵ�
    /// ����perfabObj���� ʹ��Use ���п���
    /// </summary>
    public class Equipment_Weapon : Equipment, IMCDUPC, IEquipment, IResetable
    {
        //�����ͷ��� bullet
        public GameObject BulletObj;
        public AudioClip ac;
        public GameObject Muzzle;
        public GameObject EuipmentModObj;
        public Transform firePoint;

        /// <summary>
        /// ��������ģ�� ģ��������� ���ǻ���
        /// </summary>

        public TimerNode countCDTN;

        [Button]
        public override void Execute(object obj = null)
        {

            //�ж��Ƿ�����ͷţ�CD
            if (!OnUse() || !CheckCondition()) return;

            Fire();

            Upc.GetUP(UnitPropertyTag.DelCD).GetSubType<UnitPropertyFloat>().Value = float.Parse(Upc.GetUP("CD").StringVal);
        }

        [Button]
        public virtual void Fire()
        {
            if (BulletObj == null) return;

            EventManager.Instance.SendEvent("StartFire", gameObject);
            //���
            var m = GameObjectPool.Instance.CreateObject(Muzzle.name, Muzzle, firePoint.position, firePoint.rotation);
            GameObjectPool.Instance.CollectObject(m, 2f);
            //����
            MMSoundManagerSoundPlayEvent.Trigger(ac, MMSoundManager.MMSoundManagerTracks.Sfx, firePoint.position);
            //����

            var go = GameObjectPool.Instance.CreateObject(BulletObj.name, BulletObj, firePoint.position, firePoint.rotation,null,InitBullet);
            

            for (int i = 0; i < 3; i++)
            {
                EventManager.Instance.SendEvent("Fired", gameObject, go, i);
            }

        }

        private void InitBullet(GameObject go)
        {
            go.GetComponent<MCDMgr>()?.ListUPC.Add(Upc);
            go.GetComponent<IExecutor>()?.Execute();
        }

        public override void Init()
        {
            base.Init();

            if (Upc != null)
            {
                Upc.Init();
            }
            InitCondition();
        }

        private void InitCondition()
        {
            if (DefConditionTags.Length > 0)
            {
                DefConditions = new ICondition[DefConditionTags.Length];

                for (int i = 0; i < DefConditionTags.Length; i++)
                {
                    var v = GameCommonFactory.Instance.CreateMethod<ICondition>(GameTool.GetStr("DL.Common.", DefConditionTags[i], "Condition"));

                    if (v != null)
                    {
                        DefConditions[i] = v;
                    }
                }
            }
        }

        protected override void Enable()
        {
            Use = true;

            countCDTN = TimerManager.Instance.doLoop(this, 0.2f, countCD, TimeUpdataType.Update);
        }
        protected override void Disable()
        {
            Use = false;

            if (countCDTN != null)
            {
                countCDTN.Close();
            }
        }

        protected override void countCD()
        {
            base.countCD();

        }

    }
}
