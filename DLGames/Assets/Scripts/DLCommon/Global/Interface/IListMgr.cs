using DL.MCD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ʶһ��mgr �� ����һ�� mgr�б� ������������
/// </summary>
public interface IListMgr
{
    /// <summary>
    /// listmgr name ����ָ�� listmgr
    /// </summary>
    /// <param name="name">ָ��
    /// �б���</param>
    /// <returns></returns>
    public List<MCDMgr> GetListMgr(string name = null);
}
