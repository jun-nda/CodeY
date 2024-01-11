using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {
    
    public SoftBodyController m_SoftBodyController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //获取场景中所有的particle然后进行碰撞检测
        Transform particle = m_SoftBodyController.m_Particle.transform;
        foreach (Transform child in transform) {
            if (Detect(child, particle)) {
                particle.localPosition = Vector3.zero;
            }
        }
    }

    bool Detect (Transform border, Transform particle) {
        if (particle.localPosition.y <= border.localPosition.y) {
            return true;
        }
        return false;
    }
}
