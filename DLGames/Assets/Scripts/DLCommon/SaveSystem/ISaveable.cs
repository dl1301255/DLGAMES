using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public interface ISaveable
    {
        public object Save();
        public void Load(object obj = null);
    }
}
