using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour,IWeapon {
	
	public abstract void OpenFire ();

	public Transform m_MuzzlePos; //枪口
	public Transform m_EjectionPos; // 子弹弹出位置

	public ParticleSystem m_MuzzleParticle;
	public ParticleSystem m_CasingParticle; // 弹壳特效

	protected Animator m_GunAnimator;

	public float m_FireRate;
	protected float m_LastFireTime = 0f;

	// 弹药数据相关
	public int m_AmmoEach = 30;
	public int m_AmmoAll = 120;
	protected int m_CurrentAmmo;


	protected void Awake () {
		m_CurrentAmmo = m_AmmoEach;
	}


	protected abstract void Reload ();

	protected bool IsAllowShooting () {
		return Time.time - m_LastFireTime > 1 / m_FireRate;
	}
}
