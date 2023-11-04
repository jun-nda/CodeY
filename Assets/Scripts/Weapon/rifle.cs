using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Input = UnityEngine.Windows.Input;

public class rifle : Weapon {

	public override void OpenFire () {
		if (m_CurrentAmmo <= 0) return;
		if (!IsAllowShooting()) return;
		
		m_MuzzleParticle.Play();
		m_CasingParticle.Play();
		
		m_GunAnimator.Play("Fire", 1, 0);

		CreateTrajectory();
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
	
	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			Debug.Log("OpenFire");
			OpenFire();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Reload();
		}
	}


	protected void CreateTrajectory () {
		GameObject tmp_Trajectory = Instantiate(BulletPrefab, m_MuzzlePos.position, m_MuzzlePos.rotation);
		// tmp_Trajectory.transform.SetParent(m_MuzzlePos.parent);
		Rigidbody tmp_TrajectoryRigidBody = tmp_Trajectory.AddComponent<Rigidbody>();
		// tmp_TrajectoryRigidBody.useGravity = false;
		tmp_TrajectoryRigidBody.velocity = (m_MuzzlePos.position - m_EjectionPos.position) * 1000f;
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
}