using DL.MCD;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    public class DLGMConsole : MonoBehaviour
    {
        public List<MCD.Data.UnitPropertyContainer> ListUPC;

        [Button]
        public void CreaterIns() 
        {
            var v = Common.AssetsManager.Instance.SimpleloadAsset<GameObject>("Ship_Fighter_001");
            v.GetComponent<MCD.MCDMgr>().ListUPC[0] = UPCManager.Instance.GetBaseDataUPC("Ship_001");
            Instantiate(v, Vector3.zero,Quaternion.identity);
        }

        [InlineButton("AddUPC")]
        public GameObject GetUPCByObj;
        public void AddUPC() 
        {
            if (GetUPCByObj == null) return;
            var v = GetUPCByObj.GetComponent<IMCDUPC>().GetUPC();
            if (v == null)
            {
                print("IMCDUPC == null");
                return;
            }
            UPCManager.Instance.BaseDataUPCSO.ListUPC.Add(v);
        }
       //生产一个ship
       //生产一个skill
    }
}
