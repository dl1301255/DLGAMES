using DL.MCD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMgrData : IData
{
    public string Name;
    public List<MCDMgr> ListMgr;
    public void Clear()
    {
        ListMgr.Clear();
    }

    public T GetData<T>() where T : class
    {
        return this as T;
    }

    public void Init()
    {
        if (ListMgr == null) ListMgr = new List<MCDMgr>();
    }
}
