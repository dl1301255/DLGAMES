using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public interface IEquipment : IExecutor
    {
        GameObject OwnerObj();
        void EnableInit();
        void Equip();
        void Unequip();

    }
}
