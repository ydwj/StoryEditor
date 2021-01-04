using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventManager
{
    /// <summary>
    /// 带返回参数的回调列表，支持一对多
    /// </summary>
    public static Dictionary<string, List<Delegate>> events = new Dictionary<string, List<Delegate>>();
    public static Dictionary<string, UnityEvent> eves;

    /// <summary>
    /// 注册事件，一个返回参数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    private static void CommonAdd(string eventName, Delegate callback)
    {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions))
        {
            actions.Add(callback);
        }
        else
        {
            //eventname not exist,increase an new eventstream 
            actions = new List<Delegate>();
            actions.Add(callback);
            events.Add(eventName, actions);
        }

    }

    #region 注册事件
    //注册事件，0返回参数
    public static void AddEvent(string eventName, Action callback)
    {
        CommonAdd(eventName, callback);
    }

    //一个返回参数
    public static void AddEvent<T>(string eventName, Action<T> callback)
    {
        CommonAdd(eventName, callback);
    }

    public static void AddEvent<T, T1>(string eventName, Action<T, T1> callback)
    {
        CommonAdd(eventName, callback);
    }

    public static void AddEvent<T, T1, T2>(string eventName, Action<T, T1, T2> callback)
    {
        CommonAdd(eventName, callback);
    }

    public static void AddEvent<T, T1, T2,T3>(string eventName, Action<T, T1, T2,T3> callback)
    {
        CommonAdd(eventName, callback);
    }
    #endregion

    //通用移除事件的方法
    private static void CommonRemove(string eventName, Delegate callback)
    {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions))
        {
            actions.Remove(callback);
            if (actions.Count == 0)
            {
                events.Remove(eventName);
            }
        }
    }


    #region 移除事件

    //移除事件(0参数)
    public static void RemoveEvent(string eventName, Action callback)
    {
        CommonRemove(eventName, callback);
    }

    public static void RemoveEvent<T>(string eventName, Action<T> callback)
    {
        CommonRemove(eventName, callback);
    }

    public static void RemoveEvent<T, T1>(string eventName, Action<T, T1> callback)
    {
        CommonRemove(eventName, callback);
    }

    public static void RemoveEvent<T, T1, T3>(string eventName, Action<T, T1, T3> callback)
    {
        CommonRemove(eventName, callback);
    }
    public static void RemoveEvent<T, T1, T2,T3>(string eventName, Action<T, T1,T2, T3> callback)
    {
        CommonRemove(eventName, callback);
    }

    public static void RemoveAllEvent(string eventName)
    {
        events.Clear();
    } 
    #endregion


    #region 派发事件

    public static void DispatchEvent(string eventName)
    {
        List<Delegate> actions = null;

        if (events.ContainsKey(eventName))
        {
            events.TryGetValue(eventName, out actions);

            foreach (var act in actions)
            {
                //该方法性能开销较大
                act.DynamicInvoke();
            }
        }
    }

    //1个参数
    public static void DispatchEvent<T>(string eventName, T arg)
    {
        List<Delegate> actions = null;

        if (events.ContainsKey(eventName))
        {
            //判定方法是否为空   
            events.TryGetValue(eventName, out actions);

            foreach (var act in actions)
            {
                act.DynamicInvoke(arg);
            }
        }
    }

    public static void DispatchEvent<T, T1>(string eventName, T arg, T1 arg1)
    {
        List<Delegate> actions = null;

        if (events.ContainsKey(eventName))
        {
            events.TryGetValue(eventName, out actions);

            foreach (var act in actions)
            {

                act.DynamicInvoke(arg, arg1);
            }
        }
    }

    public static void DispatchEvent<T, T1, T2>(string eventName, T arg, T1 arg1, T2 arg2)
    {
        List<Delegate> actions = null;

        if (events.ContainsKey(eventName))
        {
            events.TryGetValue(eventName, out actions);

            foreach (var act in actions)
            {

                act.DynamicInvoke(arg, arg1, arg2);
            }
        }
    }

    public static void DispatchEvent<T, T1, T2,T3>(string eventName, T arg, T1 arg1, T2 arg2,T3 arg3)
    {
        List<Delegate> actions = null;

        if (events.ContainsKey(eventName))
        {
            events.TryGetValue(eventName, out actions);

            foreach (var act in actions)
            {

                act.DynamicInvoke(arg, arg1, arg2,arg3);
            }
        }
    }
    #endregion
}
