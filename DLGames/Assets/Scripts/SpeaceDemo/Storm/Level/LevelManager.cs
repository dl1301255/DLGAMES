using DL.Common;
using DL.MCD;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Storm
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public bool Useable = false;
        public MCD.MCDMgr EnemyMgr;

        public List<Transform> ListDefGroundTF;
        public List<Transform> ListLpObjMoveTF;
        public List<LevelPart> ListLP;
        public int CrLpNum = 0;
        public Vector3 DefGroundPos;
        public float GroundSpeed;
        public List<MCDCrlSO> ListCrl;
        public TimerNode tn;


        public void Start()
        {
            if (!Useable)
            {
                initDefGroudnTFs();

                for (int i = 0; i < ListCrl.Count; i++)
                {
                    ListCrl[i].Init();
                }

                Useable = true;
            }
            ExecuteListLP();
        }

        public void ExecuteListLP ()
        {
            if (ListLP == null || ListLP.Count <= 0) return;
            ListLP[CrLpNum].transform.position = this.transform.position;
            ListLP[CrLpNum].LpStart();
            EventManager.Instance.ListenEvent<LevelPart>("LP_End",NextLPStart);
        }

        private void NextLPStart(LevelPart t1)
        {
            CrLpNum++;
            if (ListLP[CrLpNum] == null) return;
            ListLP[CrLpNum].LpStart();
        }



        private void OnDestroy()
        {
            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].Cloes();
            }
        }

        private void initDefGroudnTFs()
        {
            for (int i = 0; i < ListDefGroundTF.Count; i++)
            {
                ListDefGroundTF[i].position = new Vector3(0, DefGroundPos.y, DefGroundPos.z * i);
            }
            tn = TimerManager.Instance.doLoop(this, 0, GroundMove, TimeUpdataType.FixedUpdate);
        }
        private void GroundMove()
        {
            if (ListDefGroundTF != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    ListDefGroundTF[i].transform.Translate(Vector3.back * GroundSpeed, Space.World);

                    if (ListDefGroundTF[i].transform.position.z <= -(750 * 2))
                    {
                        var v = ListDefGroundTF[i].transform.position;
                        v.z += 750 * 4;
                        ListDefGroundTF[i].transform.position = v;
                    }
                }
            }


            if (ListLpObjMoveTF == null || ListLpObjMoveTF.Count <= 0) return;

            for (int i = ListLpObjMoveTF.Count - 1; i >= 0; i--)
            {
                if (ListLpObjMoveTF[i].gameObject.activeSelf) ListLpObjMoveTF[i].transform.Translate(Vector3.back * GroundSpeed, Space.World);
                else ListLpObjMoveTF.Remove(ListLpObjMoveTF[i]);
            }


        }
    }
}
