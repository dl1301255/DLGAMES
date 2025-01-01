using DL.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public class MCDCommonManager : MonoSingleton<MCDCommonManager>
    {
        [Title("MCD mgr crl 全局管理",
            "Init时 启动所有 Crl - 管理游戏全局规则。")]
        //Mgr Crl 管理 缓存 统一方法
        public List<MCDMgr> ListMgr;
        public List<MCDCrlSO> ListCrl;
        public DicListCrlSO ListCrlSO;

        [ShowInInspector]
        public Dictionary<string, MCDCrlSO> DicCacheCrl = new Dictionary<string, MCDCrlSO>();
        MCDCrlSO crCrlSo;
        public bool Useable = false;

        [Button]
        public override void Init()
        {
            if (Useable) return;
            else Useable = true;

            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].Init();
            }
        }
        public MCDCrlSO GetCrl(string name)
        {
            crCrlSo = null;
            if (!DicCacheCrl.TryGetValue(name, out crCrlSo))
            {
                var crCrlSo = ListCrl.Find(s => s.name == name);
                if (crCrlSo != null) 
                {
                    DicCacheCrl.Add(name, crCrlSo);
                    return crCrlSo;
                }

                var str = GameTool.GetStr("DL.MCD.Crl." + name);
                crCrlSo = GameCommonFactory.Instance.CreateObject<MCDCrlSO>(str);
                if (crCrlSo == null)
                {
                    Debug.Log("name == null CrlSO !!!");
                    return null;
                }
                DicCacheCrl.Add(name, crCrlSo);
            }
            return crCrlSo;

        }
        private void OnDestroy()
        {
            for (int i = 0; i < ListCrl.Count; i++)
            {
                ListCrl[i].Cloes();
            }
        }
    }
}
