using System;
using System.Collections.Generic;

namespace Common.UIScript
{
    public static class EventManager
    {
        private static Dictionary<string, Action<object>> eventTable = new Dictionary<string, Action<object>>();

        public static void Register(string eventName, Action<object> callback)
        {
            if (eventTable.ContainsKey(eventName))
            {
                eventTable[eventName] += callback;
            }
            else
            {
                eventTable[eventName] = callback;
            }
        }

        public static void Unregister(string eventName, Action<object> callback)
        {
            if (eventTable.ContainsKey(eventName))
            {
                eventTable[eventName] -= callback;
                if (eventTable[eventName] == null)
                {
                    eventTable.Remove(eventName);
                }
            }
        }

        public static void Notify(string eventName, object eventData = null)
        {
            if (eventTable.ContainsKey(eventName))
            {
                eventTable[eventName]?.Invoke(eventData);
            }
        }
    }
}
