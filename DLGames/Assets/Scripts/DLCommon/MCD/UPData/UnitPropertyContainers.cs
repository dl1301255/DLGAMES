using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    /// 1�����ڴ�� һ���ˡ�һ�����ߡ�һ�����������ݺϼ���
    /// 2���˵������а���һ��Bag���� ���ڱ�� ���е��ߵİڷ�˳���λ�ã����Ӹ����ݼ����л�ȡ���ݲ����档
    /// 3��ÿ����Ҫ������ĸ��壨�ˣ� ���ᱻ����һ������id �������粼�� �� ��Ҷ�ȡ���ã����µ���Ʒ���� ����Ҫid��
    /// 4��id���� = Ĭ��ֻʹ��upcName���������ظ��� upcName_id(0��1��2) �Դ�����
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
