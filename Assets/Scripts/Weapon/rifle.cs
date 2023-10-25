using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Input = UnityEngine.Windows.Input;

public class rifle : Weapon {

	public override void OpenFire () {
		if (m_CurrentAmmo <= 0) return;
		m_CurrentAmmo--;
		// if (!IsAllowShooting()) return;
		// MuzzleParticle.Play();
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
		m_LastFireTime = Time.time;
	}
	
	protected override void  Reload () {
		
	}
	
	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			OpenFire();
		}
	}
}