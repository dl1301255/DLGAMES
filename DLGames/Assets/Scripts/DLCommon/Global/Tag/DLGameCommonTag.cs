using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    /// <summary>
    /// 游戏项目 本体使用的Tag
    /// </summary>
    public class DLGameCommonTag: Common.Singleton<DLGameCommonTag>
    {
        public readonly string StartDamageEvent = "DLGameCommonTag_StartDamageEvent";
        public readonly string EndDamageEvent = "DLGameCommonTag_EndDamageEvent";
        public readonly string DLCommon = "DL.Common.";
        public readonly string SkillData = "SkillData_Tag";
        public readonly string UnitDataUPC = "UnitDataUPC_Tag";
        public readonly string LPEnd = "LevelPart_End_Tag";
        public readonly string LPStart = "LevelPart_Start_Tag";

    }
}
