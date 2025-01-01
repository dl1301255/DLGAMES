using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    [System.Serializable]
    public class SkillData
    {
        //���skill״̬ ���ڵ���
        public SkillStateEnum SkillState;
        public SkillInputType InputType;
        // �ͷż��ܵĸ���λ
        public MCDMgr SourceMgr;
        //�ͷ�Ŀ��
        public List<MCDMgr> ListTargetMgr;
        //����λ �ͷż�����ʹ�õ�����
        public UnitPropertyContainer SkillUPC;
        //����skill entity �б�
        public List<MCDMgr> ListSkillEntityMgr = new List<MCDMgr>();

        public void Init(MCDMgr mgr, UnitPropertyContainer upc, List<MCDMgr> skillEntitys)
        {
            SourceMgr = mgr;
            SkillUPC = upc;
            ListSkillEntityMgr = skillEntitys;
        }

        public void Clear()
        {
            SourceMgr = null;
            SkillUPC = null;
            ListSkillEntityMgr.Clear();
            if (ListTargetMgr != null) ListTargetMgr.Clear();
        }
        
    }
    public enum SkillInputType 
    {
        PointerDwon,PointerUP,PointerHold,DoublePointer
            //���԰�����ϰ��� execrl������������ֱ��ִ��Ч�� �������߼�
    }
}
