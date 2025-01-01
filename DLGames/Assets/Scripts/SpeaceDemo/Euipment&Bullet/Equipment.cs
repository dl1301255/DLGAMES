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
    public class Equipment : MCDMgr, IMCDUPC, IEquipment, IResetable
    {
        public bool Use = false;

        public GameObject Owner;
        public UnitPropertyContainer Upc;

        public string[] DefConditionTags;
        public ICondition[] DefConditions;
        public UnitPropertyFloat cdUP;

        public string dataType { get => this.GetType().FullName; set { return; } }
        public string ID { get => this.GetType().Name; set { return; } }

        [Button]
        public virtual void Execute(object obj = null)
        {
            //判断是否可以释放：CD
            if (!CheckCondition()) return;
        }

        protected virtual bool CheckCondition()
        {
            if (!Use) return false;

            for (int i = 0; i < DefConditions.Length; i++)
            {
               // if (!DefConditions[i].GetBool(Upc).GetValueOrDefault(false)) return false;
            }

            return true;
        }

        protected virtual void countCD()
        {
            if (!Use) return;

            if (cdUP == null)
            {
                cdUP = Upc.GetUP(UnitPropertyTag.DelCD)?.GetSubType<UnitPropertyFloat>();
            }

            if (cdUP == null || cdUP.Value <= 0) return;

            if (cdUP.Value <= 0) return;

            cdUP.Value -= 0.2f;
        }

        //装备上 和 装备卸下
        public virtual void Equip() { }

        public virtual UnitPropertyContainer GetUPC()
        {
            return Upc;
        }

        public virtual List<UnitPropertyContainer> GetUPCs()
        {
            return null;
        }

        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        public virtual UnitPropertyContainer GetUpc()
        {
            return Upc;
        }

        public override void Init()
        {
            base.Init();
            if (Upc != null)
            {
                Upc.Init();
            }
            if (DefConditionTags.Length > 0)
            {
                DefConditions = new ICondition[DefConditionTags.Length];

                for (int i = 0; i < DefConditionTags.Length; i++)
                {
                    var v = GameCommonFactory.Instance.CreateMethod<ICondition>("DL.Common." + DefConditionTags[i] + "Condition");

                    if (v != null)
                    {
                        DefConditions[i] = v;
                    }
                }
            }
        }

        public virtual void SetUPC(UnitPropertyContainer upc)
        {
            Upc = upc;
        }

        public virtual void Unequip()
        {
            return;
        }

        public virtual void OnReset()
        {
            return;
        }

        public virtual bool OnUse()
        {
            return Use;
        }
        public virtual void EnableInit()
        {
            if (Upc == null) return;
        }
        public GameObject OwnerObj()
        {
            return Owner;
        }

        public void Execute(params object[] obj)
        {
            return;
        }

        public void Close(object obj = null)
        {
            throw new NotImplementedException();
        }

        public void Close(params object[] obj)
        {
            throw new NotImplementedException();
        }
    }
}
