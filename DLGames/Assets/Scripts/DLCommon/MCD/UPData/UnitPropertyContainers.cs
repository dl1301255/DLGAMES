using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    /// 1：用于存放 一个人、一个道具、一个武器的数据合集。
    /// 2：人的数据中包含一个Bag数据 用于标记 所有道具的摆放顺序和位置，并从该数据集合中获取数据并保存。
    /// 3：每个需要被保存的个体（人） 都会被配置一个独特id 用于世界布置 和 玩家读取调用，其下的物品数据 则不需要id。
    /// 4：id规则 = 默认只使用upcName，如遇到重复则 upcName_id(0、1、2) 以此类推
    /// </summary>
    [Serializable]
    public class UnitPropertyContainers
    {
        public string UPCsName;
        public string ID;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "UPCName")]
        [Searchable]
        public List<UnitPropertyContainer> ListUPC;
        public Dictionary<string, UnitPropertyContainer> DicUPC;
        public void Init() 
        {
            ListUPC = new List<UnitPropertyContainer>();
            DicUPC = new Dictionary<string, UnitPropertyContainer>();
        }
    }
}
