using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.MCD;
using DL.Common;
using DL.MCD.Data;

public class EnemyMgr : MCDMgr, IResetable
{

    public void OnReset()
    {
        var upc = UPCManager.Instance.BaseDataUPCSO.ListUPC.Find(s => s.UPCName == ListUPC[0].UPCName);
        UPCManager.Instance.CopyInfoUPC(upc,ListUPC[0]);
    }
}
