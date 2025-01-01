using DL.MCD;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DicListCrlSO : ScriptableObject
{
    public List<ListCrlSO> ListCrlValues;
}

[System.Serializable]
public class ListCrlSO
{
    public string key;
    public List<MCDCrlSO> Crls;
}
