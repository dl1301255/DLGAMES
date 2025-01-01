using DL.MCD.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public interface IMCDUPC
    {
        public abstract UnitPropertyContainer GetUPC();
        public abstract void SetUPC(UnitPropertyContainer upc);
        public abstract List<UnitPropertyContainer> GetUPCs();
    }
}
