using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour,IWeapon {
	
	public abstract void OpenFire ();
}
