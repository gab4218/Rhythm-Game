using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Hit,
    Miss,
    Death
}

public class EventManager
{
    public delegate void EventListener();

    static Dictionary<EventType, EventListener> _events;

    public static void Subscribe(EventType type, EventListener listener)
    {
        if (_events == null) _events = new Dictionary<EventType, EventListener>();

        if (!_events.ContainsKey(type))
        {
            _events.Add(type, null);
        }
        Debug.Log("ouch");
        _events[type] += listener;
    }

    public static void Unsubscribe(EventType type, EventListener listener)
    {
        if (_events == null) return;

        if (_events.ContainsKey(type))
        {
            _events[type] -= listener;
        }

    }

    public static void TriggerEvent(EventType type)
    {
        if (_events == null)
        {
            Debug.Log("no events");
            return;
        }

        if (_events.ContainsKey(type))
        {
            if (_events[type] != null)
            {
                _events[type]();
            }
        }
    }

}
