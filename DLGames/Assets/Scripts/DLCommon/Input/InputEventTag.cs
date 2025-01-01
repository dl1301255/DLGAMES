using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.InputSys
{
    public class InputEventTag : DL.Common.Singleton<InputEventTag>
    {
        private string move;
        private string look;
        private string jumpStart;
        private string jump;
        private string jumpEnd;
        private string playerInputTag;

        /// <summary>
        /// Event Params (Tag,SenderObj,ParamsValueVector)
        /// </summary>
        public string Move { get => move; }
        /// <summary>
        /// Event Params (Tag,SenderObj,ParamsValueVector)
        /// </summary>
        public string Look { get => look;  }
        /// <summary>
        /// Event Params (name,sender,senderID,value)
        /// </summary>
        public string JumpStart { get => jumpStart; }
        /// <summary>
        /// Event Params (name,sender,senderID,value)
        /// </summary>
        public string Jump { get => jump; }
        /// <summary>
        /// Event Params (name,sender,senderID,value)
        /// </summary>
        public string JumpEnd { get => jumpEnd;  }

        /// <summary>
        /// playerInit ·¢ÉäÊÂ¼þtag£¨senderObj£¬InputTag£¬args£©
        /// </summary>
        public string PlayerInputTag { get => playerInputTag; }

        public InputEventTag()
        {
            move = GetType().FullName + "_" + "Move";
            look = GetType().FullName + "_" + "Look";
            jumpStart = GetType().FullName + "_" + "JumpStart";
            jump = GetType().FullName + "_" + "Jump";
            jumpEnd = GetType().FullName + "_" + "JumpEnd";
            playerInputTag = GetType().FullName + "_" + "PlayerInput";
        }
    }
}
