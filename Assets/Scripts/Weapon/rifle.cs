using System.Collections;
using System.Collections.Generic;
using Common.UIScript;
using UnityEngine;
// using Input = UnityEngine.Windows.Input;

public class rifle : Weapon {
	protected IEnumerator m_doAimCoroutine;
	public override void OpenFire () {
		if (m_CurrentAmmo <= 0) return;
		if (!IsAllowShooting()) return;
		
		m_MuzzleParticle.Play();
		m_CasingParticle.Play();
		
		m_GunAnimator.Play("Fire", m_IsAiming ? 1 : 0, 0);

		CreateTrajectory();
		CreateImpactEffect();
		// FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
		// FirearmsShootingAudioSource.Play();

		// mouseLook.FiringForTest();
		m_CurrentAmmo--;
		m_LastFireTime = Time.time;
	}
	
	protected override void  Reload ()
	{
		m_GunAnimatorState = m_GunAnimator.GetCurrentAnimatorStateInfo(2);
		if (m_GunAnimatorState.IsTag("ReloadAmmo")) {
			if (m_GunAnimatorState.normalizedTime > 0.0f) {
				return;
			}
		}
		if (m_CurrentAmmo == m_AmmoEach) {
			return;
		}
		
		m_GunAnimator.Play(m_CurrentAmmoAll <= 0 ? "Reload_Left" : "Reload_OutOf", 2, 0);
		StartCoroutine(CheckReloadAnimationEnd());
	}

	protected void ReloadData () {
		if ( m_CurrentAmmoAll +  m_CurrentAmmo >= m_AmmoEach)
		{
			m_CurrentAmmoAll -= m_AmmoEach - m_CurrentAmmo;
			m_CurrentAmmo = m_AmmoEach;
		}
		else
		{
			m_CurrentAmmo += m_CurrentAmmoAll;
			m_CurrentAmmoAll = 0;
		}
	}

	protected override void Aiming (bool isAiming) {
		m_IsAiming = isAiming;
		m_GunAnimator.SetBool("Aim", m_IsAiming);
		
		EventManager.SendMessage(new AimingEventArgs(isAiming));
		
		// 反复开镜的时候，不把协程停掉的话，协程始终是执行完毕的状态，所以需要先停掉然后重新起一个
		if (m_doAimCoroutine == null)
		{
			m_doAimCoroutine = DoAim();
			StartCoroutine(m_doAimCoroutine);
		}
		else
		{
			StopCoroutine(m_doAimCoroutine);
			m_doAimCoroutine = null;
			m_doAimCoroutine = DoAim();
			StartCoroutine(m_doAimCoroutine);
		}

	}
	
	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			OpenFire();
		}
		if (Input.GetMouseButtonDown(1)) {
			Aiming(true);
		}
		if (Input.GetMouseButtonUp(1))
		{
			Aiming(false);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Reload();
		}
		

	}
	
	protected void CreateTrajectory () {
		GameObject tmp_Trajectory = Instantiate(m_BulletPrefab, m_MuzzlePos.position, m_MuzzlePos.rotation);
		// tmp_Trajectory.transform.SetParent(m_MuzzlePos.parent);
		Rigidbody tmp_TrajectoryRigidBody = tmp_Trajectory.AddComponent<Rigidbody>();
		// tmp_TrajectoryRigidBody.useGravity = false;
		tmp_TrajectoryRigidBody.velocity = (m_MuzzlePos.position - m_EjectionPos.position) * 1000f;
		Destroy(tmp_Trajectory, 3);

	}

	protected void CreateImpactEffect () {
		if (Physics.Raycast(m_MuzzlePos.position, (m_MuzzlePos.position - m_EjectionPos.position).normalized,
			out RaycastHit tmp_Hit)) {
			var tmp_BulletEffect =
				Instantiate(m_ImpactPrefab,
					tmp_Hit.point,
					Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
			Destroy(tmp_BulletEffect, 3);
		}
	}
	
	protected IEnumerator CheckReloadAnimationEnd () {
		while (true) {
			yield return null;
			m_GunAnimatorState = m_GunAnimator.GetCurrentAnimatorStateInfo(2);
			if (m_GunAnimatorState.IsTag("ReloadAmmo")) {
				if (m_GunAnimatorState.normalizedTime > 0.95) {
					ReloadData();
					yield break;
				}
			}
		}
	}

	protected IEnumerator DoAim () {
		while (true) {
			yield return null;
	
			float temp_CurentFOV = 0;
			m_EyeCamera.fieldOfView = Mathf.SmoothDamp(m_EyeCamera.fieldOfView, m_IsAiming ? 45 : m_EyeOriginFOV, ref temp_CurentFOV, Time.deltaTime * 2);
		}
	}
}