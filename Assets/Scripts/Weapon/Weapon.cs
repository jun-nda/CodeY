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
	protected AnimatorStateInfo m_GunAnimatorState;
	
	public GameObject BulletPrefab;

	public float m_FireRate;
	protected float m_LastFireTime = 0f;

	// 弹药数据相关
	public int m_AmmoEach = 30;
	public int m_AmmoAll = 120;
	protected int m_CurrentAmmo;
	protected int m_CurrentAmmoAll;
	
	// 瞄准
	protected bool m_IsAiming;
	public Camera m_EyeCamera;
	protected float m_EyeOriginFOV;
	
	protected void Awake () {
		m_CurrentAmmo = m_AmmoEach;
		m_CurrentAmmoAll = m_AmmoAll;
		m_GunAnimator = transform.Find("Weapon").GetComponent<Animator>();
		m_EyeOriginFOV = m_EyeCamera.fieldOfView;

	}


	protected abstract void Reload ();
	protected abstract void Aiming (bool isAiming);
	protected bool IsAllowShooting () {
		return Time.time - m_LastFireTime > 1 / m_FireRate;
	}
}
