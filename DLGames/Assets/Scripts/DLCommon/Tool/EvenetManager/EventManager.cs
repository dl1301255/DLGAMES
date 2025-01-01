using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace DL.Common
{
    ///<summary>
    ///事件管理器
    ///<summary>
    public class EventManager : MonoSingleton<EventManager>
    {
        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<string, Dictionary<int, List<Delegate>>> DicDelegates;

        public override void Init()
        {
            base.Init();
            DicDelegates = new Dictionary<string, Dictionary<int, List<Delegate>>>();
        }
        /// <summary>
        /// 发射All事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args">默认：sender，other</param>
        public void SendEvent(string name)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                for (int i = 0; i < eventData.Count; i++)
                {
                    eventDataSended(i, eventData);
                }
            }
        }
        public void SendEvent<T1>(string name, T1 p1)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                for (int i = 0; i < eventData.Count; i++)
                {
                    eventDataSended(i, eventData, p1);
                }
            }
        }
        public void SendEvent<T1, T2>(string name, T1 p1, T2 p2)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                for (int i = 0; i < eventData.Count; i++)
                {
                    eventDataSended(i, eventData, p1, p2);
                }
            }

        }
        public void SendEvent<T1, T2, T3>(string name, T1 p1, T2 p2, T3 p3)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                for (int i = 0; i < eventData.Count; i++)
                {
                    eventDataSended(i, eventData, p1, p2, p3);
                }
            }
        }
        public void SendAllEvent<T1, T2, T3>(string name, T1 p1, T2 p2, T3 p3)
        {
            SendEvent(name);
            SendEvent(name, p1);
            SendEvent(name, p1, p2);
            SendEvent(name, p1, p2, p3);
        }

        /// <summary>
        /// 发射子事件
        /// </summary>
        /// <param name="name"> 事件名称 </param>
        /// <param name="sub"> 子编号/优先级 </param>
        /// <param name="args"> 参数 </param>
        public void SendSubEvent(string name, int sub = 0)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                eventDataSended(sub, eventData);
            }
        }
        public void SendSubEvent<T1>(string name, T1 p1, int sub = 0)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                eventDataSended(sub, eventData, p1);
            }
        }
        public void SendSubEvent<T1, T2>(string name, T1 p1, T2 p2, int sub = 0)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                eventDataSended(sub, eventData, p1, p2);
            }
        }
        public void SendSubEvent<T1, T2, T3>(string name, T1 p1, T2 p2, T3 p3, int sub = 0)
        {
            if (DicDelegates.TryGetValue(name, out var eventData))
            {
                eventDataSended(sub, eventData, p1, p2, p3);
            }
        }

        /// <summary>
        /// 收听事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eve"></param>
        /// <param name="sub"></param>
        public void ListenEvent(string name, Action eve, int sub = 0)
        {
            addDelegate(name, eve, sub);
        }
        public void ListenEvent<T1>(string name, Action<T1> eve, int sub = 0)
        {
            addDelegate(name, eve, sub);
        }
        public void ListenEvent<T1, T2>(string name, Action<T1, T2> eve, int sub = 0)
        {
            addDelegate(name, eve, sub);
        }
        public void ListenEvent<T1, T2, T3>(string name, Action<T1, T2, T3> eve, int sub = 0)
        {
            addDelegate(name, eve, sub);
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eve"></param>
        /// <param name="sub"></param>
        public void RemoveEvent(string name, Action eve, int sub = 0)
        {
            removeDelegate(name, eve, sub);
        }
        public void RemoveEvent<T1>(string name, Action<T1> eve, int sub = 0)
        {
            removeDelegate(name, eve, sub);
        }
        public void RemoveEvent<T1, T2>(string name, Action<T1, T2> eve, int sub = 0)
        {
            removeDelegate(name, eve, sub);
        }
        public void RemoveEvent<T1, T2, T3>(string name, Action<T1, T2, T3> eve, int sub = 0)
        {
            removeDelegate(name, eve, sub);
        }
        public void ClearAllEvent()
        {
            if (DicDelegates != null && DicDelegates.Count > 0) DicDelegates.Clear();
        }
        public void RemoveSubEvents(string name, int sub = 0)
        {
            if (DicDelegates.TryGetValue(name, out var eveData))
            {
                if (eveData.ContainsKey(sub))
                {
                    eveData.Remove(sub);
                }
            }
        }
        public void RemoveEvents(string name)
        {
            if (DicDelegates.ContainsKey(name))
            {
                DicDelegates.Remove(name);
            }
        }


        #region Prevate Method
        private void addDelegate(string name, Delegate eve, int sub)
        {
            if (DicDelegates.TryGetValue(name, out var eveData))
            {
                if (eveData.TryGetValue(sub, out var eveList))
                {
                    if (!eveList.Contains(eve)) eveList.Add(eve);//如果不包含这个事件，Add Eve
                }
                else
                {
                    eveData.Add(sub, CreateDelegateList(eve));
                    DicDelegates[name] = eveData.OrderBy(v => v.Key).ToDictionary(v => v.Key, v => v.Value);
                }
            }
            else
            {
                var newEveData = new Dictionary<int, List<Delegate>>();
                newEveData.Add(sub, CreateDelegateList(eve));
                DicDelegates.Add(name, newEveData);
            }
        }
        private List<Delegate> CreateDelegateList(Delegate eve)
        {
            var list = new List<Delegate>();
            list.Add(eve);
            return list;
        }
        private void removeDelegate(string name, Delegate eve, int sub)
        {
            if (DicDelegates.TryGetValue(name, out var eveData))
            {
                if (eveData.TryGetValue(sub, out var eveList))
                {
                    eveList.Remove(eve);
                }
            }
        }

        /// <summary>
        /// 发布所有sub事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        private void eventDataSended(int i, Dictionary<int, List<Delegate>> eventData)
        {
            for (int n = i; n < 10; n++)
            {
                if (eventData.ContainsKey(n))
                {
                    for (int i1 = 0; i1 < eventData[n].Count; i1++)
                    {
                        ((Action)eventData[n][i1])?.Invoke();
                    }
                    return;
                }
            }
        }
        private void eventDataSended<T1>(int i, Dictionary<int, List<Delegate>> eventData, T1 p1)
        {
            for (int n = i; n < 10; n++)
            {
                if (eventData.ContainsKey(n))
                {
                    for (int i1 = 0; i1 < eventData[n].Count; i1++)
                    {
                        ((Action<T1>)eventData[n][i1])(p1);
                    }
                    return;
                }
            }
        }
        private void eventDataSended<T1, T2>(int i, Dictionary<int, List<Delegate>> eventData, T1 p1, T2 p2)
        {
            for (int n = i; n < 10; n++)
            {
                if (eventData.ContainsKey(n))
                {
                    for (int i1 = 0; i1 < eventData[n].Count; i1++)
                    {
                        ((Action<T1, T2>)eventData[n][i1])(p1, p2);
                    }
                    return;
                }
            }
        }
        private void eventDataSended<T1, T2, T3>(int i, Dictionary<int, List<Delegate>> eventData, T1 p1, T2 p2, T3 p3)
        {
            for (int n = i; n < 10; n++)
            {
                if (eventData.ContainsKey(n))
                {
                    for (int i1 = 0; i1 < eventData[n].Count; i1++)
                    {
                        ((Action<T1, T2, T3>)eventData[n][i1])(p1, p2, p3);
                    }
                    return;
                }
            }
        }

        private void OnDisable()
        {
            ClearAllEvent();
        }
        #endregion

    }

}

