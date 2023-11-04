using System;
using System.Collections.Generic;
public class AimingEventArgs : EventArgs
{
	public bool IsAiming { get; set; }

	public AimingEventArgs(bool isAming)
	{
		IsAiming = isAming;
	}
}