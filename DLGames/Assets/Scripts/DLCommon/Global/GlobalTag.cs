using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.Common;

namespace DL.Global
{
    ///<summary>
    ///通用 游戏全局类 用于存放 游戏内 的通用事件 和 枚举
    ///<summary>
    public class GlobalTag : Singleton<GlobalTag>
    {
        #region 全局常量
        public const string UITargetSlotName = "Slot(Clone)";
        public const string UITargetSlotItemName = "SlotItem(Clone)";
        public const string ChHitNumNmae = "HitNum";
        public const string BaseInfoName = "BaseInfo.txt";
        public const string EffAffix = "_Eff";
        public const string CrlAffix = "_Crl";
        #endregion

        #region 通用事件参数
        /*敌人生成器 清空*/
        public const string StrEnemyCreatorClear = "EnemyCreatorClear";
        /*房间 清空*/
        public const string StrRoomClear = "RoomClear";
        /*RoomClear事件_AddSkill*/
        public const string RCAddSkill = "SkillFinderRC";
        /*RoomClear事件_AddSkillEnemy*/
        public const string RCAddSkillEnemy = "AddSkillEnemyRC";
        /*RoomClear事件_AddBuff*/
        public const string RCAddBuff = "BuffFinderRC";
        /*RoomClear事件_AddEnemyBuff*/
        public const string RCAddBuffEenmy = "AddBuffEnemyRC";
        /*RoomClear事件_LevelOver*/
        public const string RCLevelOver = "GoHomeRC";
        #endregion
    }
}
