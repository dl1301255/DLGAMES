using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    public enum SkillStateEnum
    {
        Init,           //初始化
        OnEquipment,    //装备中
        UnEquipment,    //卸下装备
        Execute,        //释放技能
        Update,         //更新中 time update fixupdate
        Disable,        //skill关闭
        Impact          //skill碰撞 命中
    }

}
