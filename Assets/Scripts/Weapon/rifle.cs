using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Input = UnityEngine.Windows.Input;

public class rifle : Weapon {

	public override void OpenFire () {
		if (m_CurrentAmmo <= 0) return;
		if (!IsAllowShooting()) return;
		m_MuzzleParticle.Play();
		// CurrentAmmo -= 1;
		//
		// GunAnimator.Play("Fire", IsAiming ? 1 : 0, 0);
		//
		// FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
		// FirearmsShootingAudioSource.Play();
		//
		// CreateBullet();
		// CasingParticle.Play();
		// mouseLook.FiringForTest();
		m_CurrentAmmo--;
		m_LastFireTime = Time.time;
	}
	
	protected override void  Reload ()
	{
		if (m_CurrentAmmoAll <= 0) return;
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
			OpenFire();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Reload();
		}
	}
}