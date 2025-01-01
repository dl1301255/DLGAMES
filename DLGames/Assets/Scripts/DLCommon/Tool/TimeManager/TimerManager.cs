using DL.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DL.Common
{
    public delegate void Handler();
    public delegate void Handler<T1>(T1 param1);
    public delegate void Handler<T1, T2>(T1 param1, T2 param2);
    public delegate void Handler<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

    public delegate void AdvanceTimerHnalder<T>(T timer);


    /// <summary>
    /// 时间推进器 使用接口 可以随时重写方法
    /// </summary>
    //public interface IAdvancetime
    //{
    //	void AdvanceTime();
    //   }

    ///<summary>
    ///时间管理器 用于 延迟 重复调用 
    ///计时管理器，计时器（时间线），计时节点
    ///每加载一个方法 进入计时器 当中 就是加入一个节点 ，节点包含计时方法 计时事件信息 让 使用者自省编辑节点
    ///计时器 等于一条时间线 如果需要批量管理节点 ，可将对应节点 加入执行的计时器当中
    ///<summary>
    public class TimerManager : MonoSingleton<TimerManager>
    {
        /// <summary>
        /// 主计时器
        /// </summary>
        public Timer MainTimer;
        /// <summary>
        /// 标准计时器列表
        /// </summary>
        public List<Timer> Timers = new List<Timer>();
        /// <summary>
        /// 时间推进器列表 被创建的推进器 会在Update中启动
        /// </summary>
       // public  List<IAdvancetime> advTimeList = new List<IAdvancetime>();

        //当前帧率
        private int _currFrame = 0;
        /**itmerID*/
        public int TimerIDs;
        /**nodeID*/
        public int NodeIDs;
        /** _pool 是一个缓存池 减少new 的行为*/
        [Sirenix.OdinInspector.ShowInInspector] private List<TimerNode> _pool = new List<TimerNode>();

        /// <summary>
        /// 标准时间推进器 
        /// 时间暂停控制 
        ///         暂停时间
        ///         恢复时间
        ///         暂停差值 = 恢复时间 - 暂停时间。//默认=0 有暂停时开始计算，用完后归0 
        ///         timenode.exetime += 暂停差值
        /// </summary>
        TimerNode node;
        private int count;

        public override void Init()
        {
            MainTimer = CreaterTimer();

            addMainTimer();
        }

        #region addMainTimer
        private void addMainTimer()
        {
            MainTimer.timeScale = 1;

            Timers.Add(MainTimer);
        }
        public int GetNodeID ()
        {
            return NodeIDs++;
        }

        private void Update()
        {
            foreach (var timer in Timers)
            {
                timer.UpdateAdvanceTime();
            }
        }

        private void FixedUpdate()
        {
            foreach (var timer in Timers)
            {
                timer.FixedUpdateAdvanceTime();
            }
        }
        #endregion

        public void BaseUpdateAdvanceTimer(Timer timer)
        {
            if (!timer.onUse) return;

            _currFrame++;

            checkOnUseTimeNode(timer);

            if (timer.OnPause) return;

            count = timer._nodes.Count;

            for (int i = 0; i < count; i++)
            {
                if (timer._nodes[i].timeUpdataType == TimeUpdataType.FixedUpdate ||
                    timer._nodes[i].OnPause)
                {
                    continue;
                }
                node = timer._nodes[i];
                exeNode(getTimeNodeDeltaTime(timer, node), node);
            }
        }
        private float getTimeNodeDeltaTime(Timer timer, TimerNode node)
        {
            if (node.timeUpdataType == TimeUpdataType.Frame)
            {
                return 1f;
            }
            else if(node.timeUpdataType == TimeUpdataType.FixedUpdate)
            {
                return Time.fixedDeltaTime * timer.timeScale * node.TimeScale;
            }
            else
            {
                return Time.deltaTime * timer.timeScale * node.TimeScale;
            }
           // return (node.timeUpdataType == TimeUpdataType.Frame ? currFrame : Time.deltaTime) * timer.timeScale * node.TimeScale;
        }
        public void BaseFixedUpdateAdvanceTimer(Timer timer)
        {
            if (!timer.onUse) return;

            checkOnUseTimeNode(timer);

            if (timer.OnPause) return;

            count = timer._nodes.Count;

            for (int i = 0; i < count; i++)
            {
                if (timer._nodes[i].timeUpdataType != TimeUpdataType.FixedUpdate || timer._nodes[i].OnPause)
                {
                    continue;
                }
                node = null;
                //node = timer._nodes[i];
                //var t = getTimeNodeDeltaTime(timer, node);
                //exeNode(t, node);

                exeNode(getTimeNodeDeltaTime(timer, timer._nodes[i]), timer._nodes[i]);
            }
        }

        #region exeNode
        private void exeNode(float deltaTime, TimerNode node)
        {
            node._deltaDelay += deltaTime;

            if (node._deltaDelay >= node.Delay)
            {
                if (node.repeatNum == 0)// 循环次数 num = 0 则循环
                {
                    exeNodeMethod(node);
                }
                else
                {
                    node.onRepeatNum++;
                    if (node.repeatNum <= node.onRepeatNum)
                    {
                        exeNodeMethod(node);
                        node.onUse = false;
                    }
                    else
                    {
                        exeNodeMethod(node);
                    }
                }
                if (node.Duration != 0)
                {
                    node.DeltaDuration += deltaTime;
                    if (node.DeltaDuration >= node.Duration)
                    {
                        node.onUse = false;
                        node.DeltaDuration = 0;
                    }
                }
            }
        }

        private void checkOnUseTimeNode(Timer timer)
        {
            int count = timer._nodes.Count;

            for (int i = count - 1; i >= 0; i--)//倒叙遍历
            {
                if (!timer._nodes[i].onUse)
                {
                    Clear(timer, timer._nodes[i]);
                }
            }
        }

        private void exeNodeMethod(TimerNode node)
        {
            node.method.DynamicInvoke(node.args);
            node._deltaDelay = 0;
        }
        #endregion

        /// <summary>
        /// 时间节点
        /// </summary>
        /// <param name="sender">发射源</param>
        /// <param name="useFrame">是否使用帧率</param>
        /// <param name="duration">持续时间：0 = 无限</param>
        /// <param name="repeatNum">循环次数： 0 = 无限</param>
        /// <param name="delay">间隔时间</param>
        /// <param name="method">方法</param>
        /// <param name="timer">使用的时间轴</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public TimerNode createNode(object sender, TimeUpdataType updataType, int duration, int repeatNum, int delay,
                            Delegate method, Timer timer)
        {
            var timeNode = initTimeNodeInfo(sender, updataType, repeatNum, method, ref timer);
            if (timeNode == null) return null;

            timeNode.Delay = timeNode.timeUpdataType != TimeUpdataType.Frame ? ((float)delay) / 1000 : delay;
            timeNode.Duration = timeNode.timeUpdataType != TimeUpdataType.Frame ? ((float)duration) / 1000 : duration;
            return timeNode;
        }
        public TimerNode createNode(object sender, TimeUpdataType updataType, int duration, int repeatNum, int delay,
                            Delegate method, Timer timer, params object[] args)
        {
            var node = createNode(sender, updataType, duration, repeatNum, delay, method, timer);
            node.args = args;
            return node;
        }
        public TimerNode createNode(object sender, TimeUpdataType updataType, float duration, int repeatNum, float delay,
            Delegate method, Timer timer)
        {
            var timeNode = initTimeNodeInfo(sender, updataType, repeatNum, method, ref timer);
            if (timeNode == null) return null;

            timeNode.Delay = delay;
            timeNode.Duration = duration;
            return timeNode;
        }
        public TimerNode createNode(object sender, TimeUpdataType updataType, float duration, int repeatNum, float delay,
            Delegate method, Timer timer, params object[] args)
        {
            var timeNode = initTimeNodeInfo(sender, updataType, repeatNum, method, ref timer);
            if (timeNode == null) return null;

            timeNode.Delay = delay;
            timeNode.Duration = duration;
            timeNode.args = args;
            return timeNode;
        }

        private TimerNode initTimeNodeInfo(object sender, TimeUpdataType updataType, int repeatNum, Delegate method,ref Timer timer)
        {
            if (method == null) return null;

            if (timer == null) timer = MainTimer;

            TimerNode timerNode;

            if (_pool.Count > 0)
            {
                timerNode = _pool[_pool.Count - 1];
                _pool.Remove(timerNode);
            }
            else
            {
                timerNode = new TimerNode();
            }

            timerNode.onUse = true;
            timerNode.timer = timer;
            timerNode.sender = sender;
            timerNode.timeUpdataType = updataType;
            timerNode.repeatNum = repeatNum;
            timerNode.method = method;
            timerNode.ID = NodeIDs++;
            timer._nodes.Add(timerNode);

            return timerNode;
        }
        public int currFrame
        {
            get
            {
                return _currFrame;
            }
        }
        #region 调用方法

        /// <summary>
        /// 无限循环方法 完整,ms，update，返回 TimerNode
        /// </summary>
        /// <param name="sender">发射源</param>
        /// <param name="delay">间隔(ms)</param>
        /// <param name="method">方法</param>
        /// <param name="duration">持续时间(ms)</param>
        /// <param name="repeatNum">重复次数</param>
        /// <param name="update">update更新</param>
        /// <param name="timer">计时器 (默认main)</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public TimerNode doLoop(object sender, int delay, Handler method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                int duration = 0, int repeatNum = 0, Timer timer = null)
        {
            return createNode(sender, updataType, duration, repeatNum, delay, method, timer);
        }
        public TimerNode doLoop(object sender, int delay, Handler method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, duration, repeatNum, delay, method, timer, args);
        }
        public TimerNode doLoop<T1>(object sender, int delay, Handler<T1> method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                    int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, duration, repeatNum, delay, method, timer, args);
        }
        public TimerNode doLoop<T1, T2>(object sender, int delay, Handler<T1, T2> method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                         int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            //create(false, true, delay, method, args);
            return createNode(sender, updataType, duration, repeatNum, delay, method, timer, args);
        }
        public TimerNode doLoop<T1, T2, T3>(object sender, int delay, Handler<T1, T2, T3> method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                            int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, repeatNum, duration, delay, method, timer, args);
        }

        /// <summary>
        /// float timer loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="delay"></param>
        /// <param name="method"></param>
        /// <param name="updataType"></param>
        /// <param name="durationFloat"></param>
        /// <param name="repeatNum"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        public TimerNode doLoop(object sender, float delay, Handler method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                 float durationFloat = 0, int repeatNum = 0, Timer timer = null)
        {
            return createNode(sender, updataType, durationFloat, repeatNum, delay, method, timer);
        }
        public TimerNode doLoop(object sender, float delay, Handler method, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                         float durationFloat = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, durationFloat, repeatNum, delay, method, timer, args);
        }

        /// <summary>
        /// 循环方法 次数 ms update
        /// </summary>
        /// <param name="sender">发射源</param>
        /// <param name="delay">间隔ms</param>
        /// <param name="method">方法</param>
        /// <param name="repeatNum">次数</param>
        /// <param name="timer">计时器 main</param>
        /// <param name="update">update方法</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public TimerNode doLoop_repeat(object sender, int delay, Handler method, int repeatNum = 0, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                 Timer timer = null)
        {
            return createNode(sender, updataType, 0, repeatNum, delay, method, timer);
        }
        public TimerNode doLoop_repeat(object sender, int delay, Handler method, int repeatNum = 0, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                         Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, 0, repeatNum, delay, method, timer, args);
        }
        public TimerNode doLoop_repeat<T1>(object sender, int delay, Handler<T1> method, int repeatNum = 0,
                                            TimeUpdataType updataType = TimeUpdataType.FixedUpdate, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, 0, repeatNum, delay, method, timer, args);
        }
        public TimerNode doLoop_repeat<T1, T2>(object sender, int delay, Handler<T1, T2> method, int repeatNum = 0,
                                                TimeUpdataType updataType = TimeUpdataType.FixedUpdate, Timer timer = null, params object[] args)
        {
            //create(false, true, delay, method, args);
            return createNode(sender, updataType, 0, repeatNum, delay, method, timer, args);
        }
        public TimerNode doLoop_repeat<T1, T2, T3>(object sender, int delay, Handler<T1, T2, T3> method, int repeatNum = 0,
                                                TimeUpdataType updataType = TimeUpdataType.FixedUpdate, Timer timer = null, params object[] args)
        {
            //create(false, true, delay, method, args);
            TimerNode node = createNode(sender, updataType, repeatNum, 0, delay, method, timer, args);
            return node;
        }

        /// <summary>
        /// 持续时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="delay"></param>
        /// <param name="method"></param>
        /// <param name="duration">1000ms = 1s</param>
        /// <param name="updataType"></param>
        /// <param name="timer"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public TimerNode doLoop_duration(object sender, float delay, Handler method, float duration = 0, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                        Timer timer = null, params object[] args)
        {
            //create(false, true, delay, method);
            return createNode(sender, updataType, duration, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_duration(object sender, int delay, Handler method, int duration = 0, TimeUpdataType updataType = TimeUpdataType.FixedUpdate,
                                        Timer timer = null, params object[] args)
        {
            //create(false, true, delay, method);
            return createNode(sender, updataType, duration, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_duration<T1>(object sender, int delay, Handler<T1> method, int duration = 0,
                                            TimeUpdataType updataType = TimeUpdataType.FixedUpdate, Timer timer = null, params object[] args)
        {
            //create(false, true, delay, method, args);
            return createNode(sender, updataType, duration, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_duration<T1, T2>(object sender, int delay, Handler<T1, T2> method, int duration = 0,
                                                TimeUpdataType updataType = TimeUpdataType.FixedUpdate, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, duration, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_duration<T1, T2, T3>(object sender, int delay, Handler<T1, T2, T3> method, int duration = 0,
                                                    TimeUpdataType updataType = TimeUpdataType.FixedUpdate, Timer timer = null, params object[] args)
        {
            return createNode(sender, updataType, 0, duration, delay, method, timer, args);
        }

        //快速创建 CreateTimeNode_R
        public TimerNode doLoop_express(object sender, int delay, Handler method, Timer timer = null, params object[] args)
        {
            return createNode(sender, TimeUpdataType.FixedUpdate, 0, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_express<T1>(object sender, int delay, Handler<T1> method, Timer timer = null, params object[] args)
        {
            return createNode(sender, TimeUpdataType.FixedUpdate, 0, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_express<T1, T2>(object sender, int delay, Handler<T1, T2> method, Timer timer = null, params object[] args)
        {
            return createNode(sender, TimeUpdataType.FixedUpdate, 0, 0, delay, method, timer, args);
        }
        public TimerNode doLoop_express<T1, T2, T3>(object sender, int delay, Handler<T1, T2, T3> method, Timer timer = null, params object[] args)
        {
            TimerNode node = createNode(sender, TimeUpdataType.FixedUpdate, 0, 0, delay, method, timer, args);
            return node;
        }

        /// <summary>
        /// 定时执行一次(基于帧率)
        /// </summary>
        /// <param name="delay">延迟时间(单位为帧)</param>
        /// <param name="method">结束时的回调方法</param>
        /// <param name="args">回调参数</param>
        //public void doFrameOnce(object sender, int delay, Handler method, int repeatNum = 1, Timer timer = null)
        //{
        //   // create(true, false, delay, method);
        //    createNode(sender, true, repeatNum, delay, method, timer);
        //}
        //public void doFrameOnce<T1>(object sender, int delay, Handler<T1> method, int repeatNum = 1, Timer timer = null, params object[] args)
        //{
        //    //create(true, false, delay, method, args);
        //    createNode(sender, true, repeatNum, delay, method, timer, args);
        //}
        //public void doFrameOnce<T1, T2>(object sender, int delay, Handler<T1, T2> method, int repeatNum = 1, Timer timer = null, params object[] args)
        //{
        //    //create(true, false, delay, method, args);
        //    createNode(sender, true, repeatNum, delay, method, timer, args);
        //}
        //public void doFrameOnce<T1, T2, T3>(object sender, int delay, Handler<T1, T2, T3> method, int repeatNum = 1, Timer timer = null, params object[] args)
        //{
        //    //create(true, false, delay, method, args);
        //    createNode(sender, true, repeatNum, delay, method, timer, args);
        //}

        /// <summary>
        /// 定时重复执行(基于帧率)
        /// </summary>
        /// <param name="delay">延迟时间(单位为帧)</param>
        /// <param name="method">结束时的回调方法</param>
        /// <param name="args">回调参数</param>
        public TimerNode FramedoLoop(object sender, int delay, Handler method, int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return createNode(sender, TimeUpdataType.Frame, duration, repeatNum, delay, method, timer, true, args);
        }
        public TimerNode FramedoLoop<T1>(object sender, int delay, Handler<T1> method, int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            //create(true, true, delay, method, args);
            return createNode(sender, TimeUpdataType.Frame, duration, repeatNum, delay, method, timer, true, args);
        }
        public TimerNode FramedoLoop<T1, T2>(object sender, int delay, Handler<T1, T2> method, int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return createNode(sender, TimeUpdataType.Frame, duration, repeatNum, delay, method, timer, true, args);
        }
        public TimerNode FramedoLoop<T1, T2, T3>(object sender, int delay, Handler<T1, T2, T3> method, int duration = 0, int repeatNum = 0, Timer timer = null, params object[] args)
        {
            return node = createNode(sender, TimeUpdataType.Frame, duration, repeatNum, delay, method, timer, true, args);
        }

        /// <summary>
        /// 清理定时器 - 某一种方法(多个角色 同一个脚本 同一个方法 会被统一清理)
        /// 这里应当添加一个 clearTimerHandler(timerhandler th) 这样可以只删除 自己的计时器节点
        /// </summary>
        /// <param name="method">method为回调函数本身</param>
        public void Clear(int nodeID, Timer timer = null)
        {
            if (timer == null) timer = MainTimer;
            TimerNode node = timer._nodes.Find(t => t.ID == nodeID);
            if (node == null) return;
            Clear(timer, node);
        }
        public void Clear(object sender, Handler method, Timer timer = null)
        {
            if (timer == null) timer = MainTimer;
            //Delegate del = method;
            TimerNode node = timer._nodes.Find(t => t.sender == sender && t.method == (Delegate)method);
            if (node == null) return;
            Clear(timer, node);
        }
        public void Clear(Timer timer, TimerNode node)
        {
            if (node != null)
            {
                timer._nodes.Remove(node);
                node.Clear();
                _pool.Add(node);
            }
        }
        public void Clear(TimerNode node)
        {
            Clear(MainTimer, node);
        }
        /// <summary>
        /// 清理所有定时器
        /// </summary>
        public void clearAllTimer(Timer timer = null)
        {
            if (timer == null)
            {
                timer = MainTimer;
            }
            foreach (TimerNode tn in timer._nodes)
            {
                Clear(timer, tn);
                return;
            }
        }

        public TimerNode GetTimeNode(object sender, Handler method, Timer timer = null, params object[] args)
        {
            TimerNode node;
            if (timer == null)
            {
                timer = MainTimer;
            }

            Delegate tempMethod = method;
            node = timer._nodes.Find(s => s.sender == sender && s.method == tempMethod && s.args == args);

            if (node == null) return null;
            return node;

        }
        public TimerNode GetTimeNode<T>(object sender, Handler<T> method, Timer timer = null, params object[] args)
        {
            TimerNode node;
            if (timer == null)
            {
                timer = MainTimer;
            }

            Delegate tempMethod = method;
            node = timer._nodes.Find(s => s.sender == sender && s.method == tempMethod && s.args == args);

            if (node == null) return null;
            return node;

        }

        public Timer CreaterTimer()
        {
            Timer timer = new Timer();
            return timer;
        }
        #endregion
    }

    public enum TimeUpdataType
    {
        Update, FixedUpdate, Frame
    }
}
