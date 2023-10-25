using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.UIScript;

public class WeaponAnimController : MonoBehaviour
{
	// 事件触发后 将要调用的方法
    public void PickDownFinish( )
    {
        Debug.Log("PickDownFinish");
		EventManager.SendMessage(new WeaponPickDownFinished( ));
    }
}
