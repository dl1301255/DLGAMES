using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public interface IUPCData 
    {
        public UnitPropertyContainer GetUPC();
        public void SetUPC();
        public void AddUP();
        public void RemoveUP();
    }
}
