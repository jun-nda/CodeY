using System;
using System.Collections.Generic;
public class GameStartEventArgs : EventArgs
{
    public bool Value { get; set; }

    public GameStartEventArgs(bool value)
    {
        Value = value;
    }
}