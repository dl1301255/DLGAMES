using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    public enum SkillStateEnum
    {
        Init,           //��ʼ��
        OnEquipment,    //װ����
        UnEquipment,    //ж��װ��
        Execute,        //�ͷż���
        Update,         //������ time update fixupdate
        Disable,        //skill�ر�
        Impact          //skill��ײ ����
    }

}
