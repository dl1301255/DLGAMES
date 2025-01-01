using DL.MCD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来标识一个mgr 内 包含一个 mgr列表 可以用来遍历
/// </summary>
public interface IListMgr
{
    /// <summary>
    /// listmgr name 返回指定 listmgr
    /// </summary>
    /// <param name="name">指定
    /// 列表名</param>
    /// <returns></returns>
    public List<MCDMgr> GetListMgr(string name = null);
}
