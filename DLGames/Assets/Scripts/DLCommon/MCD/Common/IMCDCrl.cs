using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    public interface IMCDCrl
    {
        public abstract MCDCrlSO GetCrl();
        public abstract List<MCDCrlSO> GetCrls();
    }
}
