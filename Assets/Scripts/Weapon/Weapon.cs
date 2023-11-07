using System.Collections.Generic;
using UnityEngine;
using Common.UIScript;
using GameData;

public abstract class Weapon : MonoBehaviour,IWeapon {
	
	public abstract void OpenFire ();

	public Transform m_MuzzlePos; //枪口
	public Transform m_EjectionPos; // 子弹弹出位置

	public ParticleSystem m_MuzzleParticle;
	public ParticleSystem m_CasingParticle; // 弹壳特效
	
	protected Animator m_GunAnimator;
	protected AnimatorStateInfo m_GunAnimatorState;
	
	public GameObject m_BulletPrefab;
	public GameObject m_ImpactPrefab;
	public float m_FireRate;
	protected float m_LastFireTime = 0f;

	// 弹药数据相关
	public int m_AmmoEach = 30;
	public int m_AmmoAll = 120;
	protected int m_CurrentAmmo;
	protected int m_CurrentAmmoAll;

	public WeaponData data;
	
	// 瞄准
	protected bool m_IsAiming;
	public Camera m_EyeCamera;
	protected float m_EyeOriginFOV;

	
	// 子弹抛壳对象池
    public Dictionary<int, PoolManager.PoolItem> bulletObjs;
	

	public CameraSpring CameraSpring;
	protected void Awake () {
		m_CurrentAmmo = m_AmmoEach;
		m_CurrentAmmoAll = m_AmmoAll;
		m_GunAnimator = transform.Find("Weapon").GetComponent<Animator>();
		m_EyeOriginFOV = m_EyeCamera.fieldOfView;

	}

	protected abstract void Reload ();

	protected abstract void Aiming (bool isAiming);

	protected bool IsAllowShooting()
	{
		return Time.time - m_LastFireTime > 1 / m_FireRate;
	}

	public bool GetIsAnim()
	{
		return m_IsAiming;
	}

	protected void FixedUpdate()
	{
		// 击中贴花

	}
}