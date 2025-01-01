using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    [System.Serializable]
    public class TimerNode
    {
        /**ID*/
        public int ID;
        //标签 = 委托方法的name
        public string TimeTag;

        /**itemr*/
        public Timer timer;
        /**执行中*/
        [ShowInInspector] private bool _onUse = false;
        public bool onPause;
        /**时间节点缩放*/
        public float timeScale = 1;
        /**寄件人*/
        public object sender;
        /**执行间隔*/
        public float _delay;
        /**执行增量*/
        public float _deltaDelay;

        /**循环次数：0 = loop / 默认 = 1 / 可set*/
        public int repeatNum;
        public int onRepeatNum;

        /// <summary>
        /// 持续时间
        /// </summary>
        public float Duration;
        public float DeltaDuration;
        //已经执行的循环

        public TimeUpdataType timeUpdataType;

        /**处理方法*/
        public Delegate method;
        /**处理方法*/
        public Handler exitHandler;

        /**参数*/
        public object[] args;

        public float Delay
        {
            get => _delay;
            set
            {
                _delay = value;
            }

        }

        /**执行中设置*/
        public bool onUse
        {
            get => _onUse;
            set
            {
                _onUse = value;
            }
        }


        public float TimeScale
        {
            get => timeScale;
            //每当被设置的时候重置一次exe时间 = lostExeTime + (cd * NodeScale * timerTimeScale),sacle = 0 触发暂停
            set
            {
                if (value == timeScale) return;
                if (timeScale == 0)
                {
                    OnPause = false;
                }
                else if(value == 0 )
                {
                    OnPause = true;
                }
                timeScale = value;
            }
        }

        public bool OnPause
        {
            get => onPause;
            set
            {
                if (onPause == value) return;

                onPause = value;
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            exitHandler?.DynamicInvoke();
            ResetTimeNode();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            onUse = false;
        }

        public void ResetTimeNode()
        {
            ID = 0;
            method = null;
            args = null;
            onRepeatNum = 0;
            TimeScale = 1;
            repeatNum = 0;
            Duration = 0;
            DeltaDuration = 0;
            onRepeatNum = 0;
            sender = null;
            exitHandler = null;
            timer = null;
            _onUse = true;
            OnPause = false;

            Delay = 0;

            onRepeatNum = 0;

        }
    }
}
