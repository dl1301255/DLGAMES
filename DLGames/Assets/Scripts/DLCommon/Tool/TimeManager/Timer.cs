using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    //[System.Serializable]
    public class Timer
    {
        /**ID*/
        public int id;
        /**执行中*/
        private bool _onUse = true;
        private bool _onPause = false;
        /**时间缩放*/
        private float _timeScale = 1;

        /// <summary>
        /// 计时节点列表
        /// </summary>
        public List<TimerNode> _nodes = new List<TimerNode>();

        /// <summary>
        /// 计时推进器
        /// </summary>
        public AdvanceTimerHnalder<Timer> UpdateAdvTimerHandler;
        public AdvanceTimerHnalder<Timer> FixedUpdateAdvTimerHandler;

        public Timer()
        {
            _onUse = true;
            _onPause = false;
        }
        public bool onUse
        {
            get => _onUse;
            set
            {
                if (_onUse == value) return;
                _onUse = value;
            }
        }
        public float timeScale
        {
            get => _timeScale;
            set
            {
                if (value == _timeScale) return;

                if (value == 0)
                {
                    OnPause = true;
                }
                else
                {
                    OnPause = false;
                }

                _timeScale = value;

            }
        }

        public float DeltaTime
        {
            get => Time.deltaTime * timeScale;
        }
        public bool OnPause
        {
            get => _onPause;
            set
            {
                if (_onPause == value) return;
                _onPause = value;
            }
        }

        public virtual void UpdateAdvanceTime()
        {
            if (!onUse) return;

            if (UpdateAdvTimerHandler == null)
            {
                TimerManager.Instance.BaseUpdateAdvanceTimer(this);

            }
            else
            {
                UpdateAdvTimerHandler?.Invoke(this);
            }


        }
        public virtual void FixedUpdateAdvanceTime()
        {
            if (!onUse) return;

            if (FixedUpdateAdvTimerHandler == null)
            {
                TimerManager.Instance.BaseFixedUpdateAdvanceTimer(this);
            }
            else
            {
                FixedUpdateAdvTimerHandler?.Invoke(this);
            }


        }
        /**删除timer*/
        public void clear() { }
    }
}
