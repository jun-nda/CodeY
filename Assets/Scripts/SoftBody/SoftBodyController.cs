using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBodyController : MonoBehaviour {
    
    public GameObject m_ParticlePrefab;

    public GameObject m_Particle;

    public float m_Velocity = 5f;
    void Awake () {
        m_Particle = Instantiate(m_ParticlePrefab);
    }

    // Start is called before the first frame update
    void Start() {
        m_Particle.transform.localPosition = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = m_Particle.transform.localPosition;
        m_Particle.transform.localPosition -= new Vector3(pos.x, m_Velocity * Time.deltaTime, pos.z);
    }
}
