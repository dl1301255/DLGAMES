using DL.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DL.Common
{
    public class MainUIWindow : UIWindow
    {
        [DetailedInfoBox("info:", 
            "��һ�У��ر�ui����������ģʽ\n"  +
            "�ڶ��У�ѡ��ship��ѡ���ʼװ����ѡ��ؿ���ѡ����ս���ؿ��Ѷ����� �� ������Ĺؿ����ӣ�\n" +
            "�����У�config������ ��Ч ���� ���������˳���Ϸ")]
        public string Name;


        public void CloseCrWindow() 
        {
            gameObject.SetActive(false);
        }

        public override void CloseWindow()
        {
            throw new System.NotImplementedException();
        }

        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public override void OpenWindow()
        {
            if (!enabled) this.enabled = true;
            
        }
    }

}
