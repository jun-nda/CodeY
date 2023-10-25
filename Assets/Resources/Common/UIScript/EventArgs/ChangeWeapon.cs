using System;
using System.Collections.Generic;
public class ChangeWeapon : EventArgs
{
    public string WeaponNmae { get; set; }

    public ChangeWeapon(string weaponNmae)
    {
        WeaponNmae = weaponNmae;
    }
}