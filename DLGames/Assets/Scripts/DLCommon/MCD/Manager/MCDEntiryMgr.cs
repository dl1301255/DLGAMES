using DL.MCD;
using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    /// <summary>
    /// 传入一个UPC 
    /// 根据UPC 执行特定的方法
    /// </summary>
    public class MCDEntiryMgr : MCDMgr,IMCDUPC
    {
        public UnitPropertyContainer UPC;

        public UnitPropertyContainer GetUPC()
        {
            return UPC;
        }

        public List<UnitPropertyContainer> GetUPCs()
        {
            throw new System.NotImplementedException();
        }

        public void SetUPC(UnitPropertyContainer upc)
        {
            UPC = upc;
        }
    }
}
