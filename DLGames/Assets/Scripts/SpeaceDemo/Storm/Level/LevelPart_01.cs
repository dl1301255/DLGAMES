using DL.Common;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Storm
{
    public class LevelPart_01 : LevelPart
    {
        public Transform enemyPos;
        public Vector2 enemyPosOffset;
        public int crNum;
        public int maxNum;

        public float createrCD;
        public float createrCDDel;

        public List<string> DefEnemyName;

        public override void Init()
        {
            base.Init();
            if (!Use)
            {
                Use = true;
            }
        }

        public override void LpStart()
        {
            //Éú³ÉµÐÈË
            //TimerManager.Instance.doLoop(this, 0, lploop, TimeUpdataType.Update);
            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].ExecuteCrl(this);
            }
        }

        public override void LpEnd()
        {
            LevelManager.Instance.ListLpObjMoveTF.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(enemyPos.position, new Vector3(enemyPosOffset.x, 0, enemyPosOffset.y));
        }


    }
}
