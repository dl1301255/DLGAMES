using DL.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DL.Common
{
    //标记 Obj身上的nodes
    public interface ITimerNodes
    {
        List<TimerNode> GetTimerNodes();
    }
}