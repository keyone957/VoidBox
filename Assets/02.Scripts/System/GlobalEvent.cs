using System.Collections.Generic;
using System;
using UnityEngine;

public enum EventType
{
    OnPlayerHealthDecreased,    //Player에게 데미지 적용
    OnPlayerHitEffect,          //HitEffect

    UpdateWeaponUI,
    PlayerDied,                 //플레이어가 죽었을 때
}

public class GlobalEvent : MonoBehaviour
{
    public static GlobalEvent Instance;
    private static Dictionary<int, Delegate> eventDictionary = new Dictionary<int, Delegate>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    #region Event Registration
    public static void RegisterEvent(EventType type, Action action)
    {
        int key = (int)type;
        if (eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = Delegate.Combine(eventDictionary[key], action);
        }
        else
        {
            eventDictionary.Add(key, action);
        }
    }

    public static void RegisterEvent<T>(EventType type, Action<T> action)
    {
        int key = (int)type;
        if (eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = Delegate.Combine(eventDictionary[key], action);
        }
        else
        {
            eventDictionary.Add(key, action);
        }
    }

    public static void RegisterEvent<T1, T2>(EventType type, Action<T1, T2> action)
    {
        int key = (int)type;
        if (eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = Delegate.Combine(eventDictionary[key], action);
        }
        else
        {
            eventDictionary.Add(key, action);
        }
    }

    public static void RegisterEvent<T1, T2, T3>(EventType type, Action<T1, T2, T3> action)
    {
        int key = (int)type;
        if (eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = Delegate.Combine(eventDictionary[key], action);
        }
        else
        {
            eventDictionary.Add(key, action);
        }
    }
    #endregion

    #region Event Removal
    public static void RemoveEvent(EventType type, Action action)
    {
        int key = (int)type;
        if (eventDictionary.TryGetValue(key, out Delegate existingDelegate))
        {
            existingDelegate = Delegate.Remove(existingDelegate, action);
            if (existingDelegate == null) eventDictionary.Remove(key);
            else eventDictionary[key] = existingDelegate;
        }
    }

    public static void RemoveEvent<T>(EventType type, Action<T> action)
    {
        int key = (int)type;
        if (eventDictionary.TryGetValue(key, out Delegate existingDelegate))
        {
            existingDelegate = Delegate.Remove(existingDelegate, action);
            if (existingDelegate == null) eventDictionary.Remove(key);
            else eventDictionary[key] = existingDelegate;
        }
    }

    public static void RemoveEvent<T1, T2>(EventType type, Action<T1, T2> action)
    {
        int key = (int)type;
        if (eventDictionary.TryGetValue(key, out Delegate existingDelegate))
        {
            existingDelegate = Delegate.Remove(existingDelegate, action);
            if (existingDelegate == null) eventDictionary.Remove(key);
            else eventDictionary[key] = existingDelegate;
        }
    }

    public static void RemoveEvent<T1, T2, T3>(EventType type, Action<T1, T2, T3> action)
    {
        int key = (int)type;
        if (eventDictionary.TryGetValue(key, out Delegate existingDelegate))
        {
            existingDelegate = Delegate.Remove(existingDelegate, action);
            if (existingDelegate == null) eventDictionary.Remove(key);
            else eventDictionary[key] = existingDelegate;
        }
    }
    #endregion

    #region EventCall
    public static void CallEvent(EventType eventType)
    {
        int key = (int)eventType;
        if (eventDictionary.TryGetValue(key, out Delegate del))
        {
            if (del is Action action) action.Invoke();
        }
        else
        {
            Debug.LogWarning($"Event not registered: {eventType}");
        }
    }

    public static void CallEvent<T>(EventType eventType, T param)
    {
        int key = (int)eventType;
        if (eventDictionary.TryGetValue(key, out Delegate del))
        {
            if (del is Action<T> action) action.Invoke(param);
        }
        else
        {
            Debug.LogWarning($"Event not registered: {eventType}");
        }
    }

    public static void CallEvent<T1, T2>(EventType eventType, T1 param1, T2 param2)
    {
        int key = (int)eventType;
        if (eventDictionary.TryGetValue(key, out Delegate del))
        {
            if (del is Action<T1, T2> action) action.Invoke(param1, param2);
        }
        else
        {
            Debug.LogWarning($"Event not registered: {eventType}");
        }
    }

    public static void CallEvent<T1, T2, T3>(EventType eventType, T1 param1, T2 param2, T3 param3)
    {
        int key = (int)eventType;
        if (eventDictionary.TryGetValue(key, out Delegate del))
        {
            if (del is Action<T1, T2, T3> action) action.Invoke(param1, param2, param3);
        }
        else
        {
            Debug.LogWarning($"Event not registered: {eventType}");
        }
    }
    #endregion
}
