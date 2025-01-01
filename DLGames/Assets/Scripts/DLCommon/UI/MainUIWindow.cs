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
            "第一行：关闭ui、？？其他模式\n"  +
            "第二行：选择ship、选择初始装备、选择关卡、选择挑战（关卡难度设置 和 保存过的关卡种子）\n" +
            "第三行：config（声音 特效 文字 按键）、退出游戏")]
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
