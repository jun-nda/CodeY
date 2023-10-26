using System;
using System.Collections.Generic;
public class ChangeWeapon : EventArgs
{
    public WeaponType WeaponType { get; set; }

    public ChangeWeapon(WeaponType weaponType)
    {
        WeaponType = weaponType;
    }
}