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
        //��ǩ = ί�з�����name
        public string TimeTag;

        /**itemr*/
        public Timer timer;
        /**ִ����*/
        [ShowInInspector] private bool _onUse = false;
        public bool onPause;
        /**ʱ��ڵ�����*/
        public float timeScale = 1;
        /**�ļ���*/
        public object sender;
        /**ִ�м��*/
        public float _delay;
        /**ִ������*/
        public float _deltaDelay;

        /**ѭ��������0 = loop / Ĭ�� = 1 / ��set*/
        public int repeatNum;
        public int onRepeatNum;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public float Duration;
        public float DeltaDuration;
        //�Ѿ�ִ�е�ѭ��

        public TimeUpdataType timeUpdataType;

        /**������*/
        public Delegate method;
        /**������*/
        public Handler exitHandler;

        /**����*/
        public object[] args;

        public float Delay
        {
            get => _delay;
            set
            {
                _delay = value;
            }

        }

        /**ִ��������*/
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
            //ÿ�������õ�ʱ������һ��exeʱ�� = lostExeTime + (cd * NodeScale * timerTimeScale),sacle = 0 ������ͣ
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
        /// ���
        /// </summary>
        public void Clear()
        {
            exitHandler?.DynamicInvoke();
            ResetTimeNode();
        }

        /// <summary>
        /// �ر�
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
