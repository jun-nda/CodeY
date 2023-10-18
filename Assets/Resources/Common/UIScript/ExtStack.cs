using System.Collections.Generic;

namespace Common.UIScript
{
    /// <summary>
    /// 自定义的扩展Stack，以list为基础。在Stack基础上增加Pop特定value 接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtStack<T>
    {
        private readonly List<T> list = new List<T>();

        public T Peek()
        {
            if (Count > 0)
            {
                return list[Count - 1];
            }

            return default;
        }

        public void Push(T t)
        {
            list.Add(t);
        }

        public T Pop()
        {
            if (Count > 0)
            {
                var idx = Count - 1;
                var ret = list[idx];
                list.RemoveAt(idx);
                return ret;
            }

            return default;
        }

        public T Pop(T t)
        {
            if (Count > 0)
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    var v = list[i];
                    if (v.Equals(t))
                    {
                        list.RemoveAt(i);
                        return t;
                    }
                }
            }

            return t;
        }

        public int Count => list.Count;
    }
}