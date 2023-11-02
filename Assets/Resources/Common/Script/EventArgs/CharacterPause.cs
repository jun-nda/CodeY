using System;
using System.Collections.Generic;
public class CaharacterPause : EventArgs
{
    public bool Value { get; set; }

    public CaharacterPause(bool value)
    {
        Value = value;
    }
}