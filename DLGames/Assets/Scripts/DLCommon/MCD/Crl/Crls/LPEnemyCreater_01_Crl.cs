using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD.Data;
using DL.Common;
using DL.Global;
using Sirenix.OdinInspector;
using System.Linq;
using DL.Storm;

namespace DL.MCD.Crl
{
    public class LPEnemyCreater_01_Crl : LPEnemeyCreaterCrl
    {
        public Vector3 enemyPos;
        public Vector3 enemyPosOffset;
        public float CreaterCD;

        public float MaxStrength;
        //�������ĵ�����С����
        public int MinNum;
        //����ʱ �жϵ���ǿ�ȸߵ͵ķ�Χ
        [Range(0, 10)]
        public int StrengthRangue;
        [Range(0, 1)]
        public float StrengthRandomProportion;
        /// <summary>
        /// �����ɵ� ͬʱ�ڳ� �����
        /// </summary>
        public float CrMaxNum;


        public float configTotalStrengt;
        public int creatableNum;
        public int crNum;
        public float createrCDDel;

        public List<string> listDefEnemey;
        public List<GameObject> ListCrEnemy;
        public List<UnitEnemyCreaterData> ListEnemyData;
        public MCDMgr McdMgr;
        private TimerNode tn;

        [Button]
        public override void Init()
        {
            ListEnemyData = new List<UnitEnemyCreaterData>();
            enemyPoolConfig();
            //����unit�����¼� ѯ���Ƿ���listCrEnemy ����� ��ȥ�� �� Crnum-- ֱ��û��
        }

        private void enemyPoolConfig()
        {

            for (int i = 0; i < listDefEnemey.Count; i++)
            {
                var v = UPCManager.Instance.GetBaseDataUPC(listDefEnemey[i]);
                var stength = v.GetUP("Stength")?.stringVal;
                if (string.IsNullOrEmpty(stength)) continue;


                ListEnemyData.Add(new UnitEnemyCreaterData());

                ListEnemyData[i].obj = AssetsManager.Instance.LoadAsset<GameObject>(v.UPCName);
                ListEnemyData[i].Stength = float.Parse(stength);
                ListEnemyData[i].Name = listDefEnemey[i];
                ListEnemyData[i].Upc = v;
            }


            var List = ListEnemyData.OrderBy(s => s.Stength).ToList();
            ListEnemyData = List;

            var randomStength = MaxStrength * StrengthRandomProportion;

            //�������
            while (configTotalStrengt < randomStength)
            {
                randomCreaterEnemy(1, ListEnemyData.Count);
            }

            //������С�� �ӵ���ǿ�� �͵������ 
            while (MinNum > creatableNum)
            {
                randomCreaterEnemy(1, 1 + StrengthRangue);
            }


            //�����ǿ
            while (configTotalStrengt < MaxStrength)
            {
                randomCreaterEnemy(ListEnemyData.Count - StrengthRangue, ListEnemyData.Count);
            }
        }

        private void randomCreaterEnemy(int min, int max)
        {
            var num = GameTool.GetRandomNum(min, max) - 1;
            ListEnemyData[num].CreatableNum += 1;
            creatableNum += 1;
            if (ListEnemyData[num].Stength <= 0) ListEnemyData[num].Stength = 1;
            configTotalStrengt += ListEnemyData[num].Stength;
        }

        private void createrEenmey(int num)
        {
            if (ListEnemyData[num].CreatableNum <= 0) return;
            GameObject obj = AssetsManager.Instance.LoadAsset<GameObject>(ListEnemyData[num].Name);

            if (obj == null) return;
            Vector3 pos = new Vector3(Random.Range(-enemyPosOffset.x, enemyPosOffset.x) + McdMgr.transform.position.x, 0f,
                                Random.Range(-enemyPosOffset.y, enemyPosOffset.y) + McdMgr.transform.position.z);
            var o = GameObjectPool.Instance.CreateObject(ListEnemyData[num].Name, obj, pos + enemyPos, McdMgr.transform.rotation);
            ListCrEnemy.Add(o);
            ListEnemyData[num].CreatableNum--;
            crNum++;
            creatableNum--;
            LevelManager.Instance.ListLpObjMoveTF.Add(o.transform);
        }

        [Button]
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr != null) McdMgr = mgr;
            if (McdMgr == null) return;

            //����������� ��������
            tn = TimerManager.Instance.doLoop(this, 0, exe, TimeUpdataType.Update);
        }

        private void exe()
        {
            //����CDdel ���num ���CD
            if (createrCDDel < CreaterCD)
            {
                createrCDDel += Time.deltaTime;
            }
            if (crNum >= CrMaxNum || createrCDDel < CreaterCD || creatableNum <= 0) return;
            var num = GameTool.GetRandomNum(0, ListEnemyData.Count - 1);
            getUsableEnemyData(num);
            createrCDDel = 0;
        }

        private void getUsableEnemyData(int num)
        {
            int numable = 0;

            for (int i = 0; i < ListEnemyData.Count; i++)
            {
                numable = num + i >= ListEnemyData.Count ? ListEnemyData.Count - 1 : num + i;
                if (ListEnemyData[numable].CreatableNum > 0)
                {
                    createrEenmey(numable);
                    return;
                }

                numable = num - i <= 0 ? 0 : num - i;
                if (ListEnemyData[numable].CreatableNum > 0)
                {
                    createrEenmey(numable);
                    return;
                }

            }
        }

        public override void EndExecuteCrl(MCDMgr mgr = null)
        {
            if (tn != null) TimerManager.Instance.Clear(tn);
            Clear();
        }
        [Button]
        public override void Cloes()
        {
            Clear();
        }
        private void Clear()
        {
            configTotalStrengt = 0;
            crNum = 0;
            createrCDDel = 0;
            creatableNum = 0;
            ListEnemyData.Clear();
            ListCrEnemy.Clear();
        }
    }

}
