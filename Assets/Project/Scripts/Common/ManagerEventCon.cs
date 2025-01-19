using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ProEventType
{
    Init,
    Lan_CN,
    Lan_EN,
    Btn_Next,
    Btn_Home,
    Btn_Back,
    ChangeType,
    FinishEvent,
    DeathEvent,
    Btn_BackgroundMusic,
    SoundEffects
}

public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate void CallBack<T, X>(T arg1, X arg2);

public static class ManagerEventCon
{
    private static Dictionary<ProEventType, List<Delegate>> ProEventTable = new Dictionary<ProEventType, List<Delegate>>();

    public static void AddListener(ProEventType eventType, CallBack callback)
    {
        if (!ProEventTable.ContainsKey(eventType))
        {
            ProEventTable.Add(eventType, null);
            ProEventTable[eventType] = new List<Delegate>(3);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
        }

        ProEventTable[eventType][0] = (CallBack)ProEventTable[eventType][0] + callback;
    }

    public static void AddListener<T>(ProEventType eventType, CallBack<T> callback)
    {
        if (!ProEventTable.ContainsKey(eventType))
        {
            ProEventTable.Add(eventType, null);
            ProEventTable[eventType] = new List<Delegate>(3);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
        }

        ProEventTable[eventType][1] = (CallBack<T>)ProEventTable[eventType][1] + callback;
    }

    public static void AddListener<T, X>(ProEventType eventType, CallBack<T, X> callback)
    {
        if (!ProEventTable.ContainsKey(eventType))
        {
            ProEventTable.Add(eventType, null);
            ProEventTable[eventType] = new List<Delegate>(3);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
        }

        ProEventTable[eventType][2] = (CallBack<T, X>)ProEventTable[eventType][2] + callback;
    }

    public static void RemoveListener(ProEventType eventType, CallBack callback)
    {
        if (ProEventTable.ContainsKey(eventType))
        {
            ProEventTable[eventType][0] = (CallBack)ProEventTable[eventType][0] - callback;
        }
    }

    public static void RemoveListener<T>(ProEventType eventType, CallBack<T> callback)
    {
        if (ProEventTable.ContainsKey(eventType))
        {
            ProEventTable[eventType][1] = (CallBack<T>)ProEventTable[eventType][1] - callback;
        }
    }

    public static void RemoveListener<T, X>(ProEventType eventType, CallBack<T, X> callback)
    {
        if (ProEventTable.ContainsKey(eventType))
        {
            ProEventTable[eventType][2] = (CallBack<T, X>)ProEventTable[eventType][2] - callback;
        }
    }

    public static void RemoveAllListener(ProEventType eventType)
    {
        if (ProEventTable.ContainsKey(eventType))
        {
            ProEventTable[eventType] = new List<Delegate>(3);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
            ProEventTable[eventType].Add(null);
        }
    }

    public static void BroadCast(ProEventType eventType)
    {
        List<Delegate> d;
        if (ProEventTable.ContainsKey(eventType))
        {
            ProEventTable.TryGetValue(eventType, out d);
            {
                CallBack callBack = d[0] as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
            }
        }
    }

    public static void BroadCast<T>(ProEventType eventType, T arg)
    {
        List<Delegate> d;
        if (ProEventTable.ContainsKey(eventType))
        {
            ProEventTable.TryGetValue(eventType, out d);
            {
                CallBack<T> callBack = d[1] as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
            }
        }
    }

    public static void BroadCast<T, X>(ProEventType eventType, T arg1, X arg2)
    {
        List<Delegate> d;
        ProEventTable.TryGetValue(eventType, out d);
        {
            CallBack<T, X> callBack = d[2] as CallBack<T, X>;
            if (callBack != null)
            {
                callBack(arg1, arg2);
            }
        }
    }

    public static bool GetListenerCount(ProEventType eventType)
    {
        if (ProEventTable.ContainsKey(eventType))
        {
            if (ProEventTable[eventType] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
