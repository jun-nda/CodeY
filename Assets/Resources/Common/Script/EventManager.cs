using System;
using System.Collections.Generic;

namespace Common.UIScript
{
    public static class EventManager
    {
        private static Dictionary<Type, LinkedList<Delegate>> eventTable = new Dictionary<Type, LinkedList<Delegate>>();

        public static void AddListener<T>(Action<T> listener) where T : EventArgs
        {
            Type eventType = typeof(T);
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, new LinkedList<Delegate>());
            }

            eventTable[eventType].AddLast(listener);
        }

        public static void RemoveListener<T>(Action<T> listener) where T : EventArgs
        {
            Type eventType = typeof(T);
            if (eventTable.ContainsKey(eventType))
            {
                LinkedListNode<Delegate> node = eventTable[eventType].First;
                while (node != null)
                {
                    if (node.Value.Equals(listener))
                    {
                        eventTable[eventType].Remove(node);
                        break;
                    }

                    node = node.Next;
                }
            }
        }

        public static void SendMessage<T>(T eventArgs) where T : EventArgs
        {
            Type eventType = typeof(T);
            if (eventTable.ContainsKey(eventType))
            {
                LinkedListNode<Delegate> node = eventTable[eventType].First;
                while (node != null)
                {
                    ((Action<T>)node.Value).Invoke(eventArgs);
                    node = node.Next;
                }
            }
        }
    }
}
