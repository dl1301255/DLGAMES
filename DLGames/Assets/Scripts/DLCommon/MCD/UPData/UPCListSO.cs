using DL.MCD.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    
    public class UPCListSO : ScriptableObject
    {
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "UPCName")]
        [Searchable]
        public List<UnitPropertyContainer>  ListUPC = new List<UnitPropertyContainer>();

        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "UPCsName")]
        [Searchable]
        public List<UnitPropertyContainers> ListUPCS = new List<UnitPropertyContainers>();

        public Dictionary<string, UnitPropertyContainer> DicUPC = new Dictionary<string, UnitPropertyContainer>();
    }
}
