using System;
using System.Collections.Generic;

public static class EventBus {
    private static readonly Dictionary<Type, List<Delegate>> eventTable = new();
    private static readonly Dictionary<Delegate, Delegate> callbackMap = new(); // ⭐ 映射表，用于支持无参回调取消订阅

    public static void Subscribe<T>(Action<T> callback) {
        var type = typeof(T);
        if (!eventTable.ContainsKey(type))
            eventTable[type] = new List<Delegate>();

        eventTable[type].Add(callback);
    }

    // ⭐ 重载：支持无参 Action 自动包装为 Action<T>
    public static void Subscribe<T>(Action callback) {
        Action<T> wrapper = _ => callback();
        callbackMap[callback] = wrapper;
        Subscribe(wrapper);
    }

    public static void Unsubscribe<T>(Action<T> callback) {
        var type = typeof(T);
        if (eventTable.TryGetValue(type, out var list)) {
            list.Remove(callback);
            if (list.Count == 0)
                eventTable.Remove(type);
        }
    }

    // ⭐ 重载：支持移除无参 Action 的包装版本
    public static void Unsubscribe<T>(Action callback) {
        if (callbackMap.TryGetValue(callback, out var wrapper)) {
            Unsubscribe((Action<T>)wrapper);
            callbackMap.Remove(callback);
        }
    }

    public static void Publish<T>(T evt) {
        var type = typeof(T);
        if (eventTable.TryGetValue(type, out var list)) {
            foreach (var callback in list)
                ((Action<T>)callback).Invoke(evt);
        }
    }

    public static void ClearAll() {
        eventTable.Clear();
        callbackMap.Clear();
    }

}

public struct OverdriveEvent {
    public bool IsOn;
}

public struct GameOverEvent { }

