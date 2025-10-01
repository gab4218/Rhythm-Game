using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Hit,
    Miss,
    Death,
    End
}

public class EventManager
{
    public delegate void EventListener(params object[] paramContainer);

    static Dictionary<EventType, EventListener> _events;


    public static void Subscribe(EventType type, EventListener listener)
    {
        if (_events == null)
        {
            _events = new Dictionary<EventType, EventListener>();
            //Subscribe(EventType.Death, ClearEvents);
        }

        if (!_events.ContainsKey(type))
        {
            _events.Add(type, null);
        }
        //Debug.Log("ouch");
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

    public static void TriggerEvent(EventType type, params object[] paramContainer)
    {
        if (_events == null || _events.Keys.Count == 0)
        {
            Debug.Log("no events");
            return;
        }

        if (_events.ContainsKey(type))
        {
            if (_events[type] != null)
            {
                _events[type](paramContainer);
            }
        }
    }

    public static void TriggerEvent(EventType type)
    {
        TriggerEvent(type, null);
    }

    //public static void ClearEvents(params object[] paramContainer)
    //{
    //    _events.Clear();
    //    Subscribe(EventType.Death, ClearEvents);
    //}

}
