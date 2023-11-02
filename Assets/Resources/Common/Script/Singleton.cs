using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Singleton<T> where T : new()
{
    private static T inst;

    public static T Inst
    {
        get
        {
            if (inst == null)
            {
                inst = new T();
            }
            return inst;
        }
    }
}
