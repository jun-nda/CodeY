using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoftBodyController : MonoBehaviour {
    
    public GameObject m_ParticlePrefab;
    public GameObject m_SpringPrefab;
    
    public GameObject m_Particle;
    public GameObject m_Particle2;
    public GameObject m_Spring;
    public Material m_ParticleMat;
    public float m_Velocity = 5f;
    void Awake () {
        m_Particle = Instantiate(m_ParticlePrefab);
        m_Particle2 = Instantiate(m_Particle2);
        
        m_Spring = Instantiate(m_SpringPrefab);
    }

    // Start is called before the first frame update
    void Start() {
        m_Particle.transform.localPosition = new Vector3(0, 10, 0);
        m_Particle2.transform.localPosition = new Vector3(2, 9, 0);
        
        m_Spring.transform.localPosition = new Vector3(1, 9.5f, 0);
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = m_Particle.transform.localPosition;
        m_Particle.transform.localPosition -= new Vector3(pos.x, m_Velocity * Time.deltaTime, pos.z);

        DrawCircle(pos, 0.1f, 20);
        
        Vector3 pos2 = m_Particle.transform.localPosition;
        m_Particle2.transform.localPosition -= new Vector3(pos2.x, m_Velocity * Time.deltaTime, pos2.z);

        DrawCircle(pos2, 0.1f, 20);

        // DrawSquare(pos, pos2);
    }

    void DrawCircle(Vector3 center, float radius, int segment)
    {
        MeshFilter meshFilter =  m_Particle.GetComponent<MeshFilter>();
        MeshRenderer renderer = m_Particle.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = m_Particle.GetComponent<MeshCollider>();
        renderer.material = m_ParticleMat;
        
        //顶点
        Vector3[] vertices = new Vector3[segment + 1];
        vertices[0] = center;
        float deltaAngle = Mathf.Deg2Rad * 360f / segment;
        float currentAngle = 0;
        for (int i = 1; i < vertices.Length; i++)
        {
            float cosA = Mathf.Cos(currentAngle);
            float sinA = Mathf.Sin(currentAngle);
            vertices[i] = new Vector3(cosA * radius + center.x, sinA * radius + center.y, 0);
            currentAngle += deltaAngle;
        }

        //三角形
        int[] triangles = new int[segment * 3];
        for (int i = 0, j = 1; i < segment * 3 - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segment * 3 - 3] = 0;
        triangles[segment * 3 - 2] = 1;
        triangles[segment * 3 - 1] = segment;

        //===========================UV============================
        Vector2[] newUV = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            newUV[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].y / radius / 2 + 0.5f);
        }
        
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.uv = newUV;
        mesh.triangles = triangles;
        
       
        meshCollider.sharedMesh = mesh;
        
    }
    
    // //假装是画弹簧
    // void DrawSquare(Vector3 pos1, Vector3 pos2)
    // {
    //     MeshFilter meshFilter =  m_Spring.GetComponent<MeshFilter>();
    //     MeshRenderer renderer = m_Spring.GetComponent<MeshRenderer>();
    //     renderer.sharedMaterial = m_ParticleMat;
    //
    //     Mesh mesh = meshFilter.mesh;
    //     mesh.Clear();
    //
    //     Vector3 vertex1 = pos1 + 
    //     
    //     mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
    //     Vector2[] newUV = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };//设置UV。
    //     mesh.uv = newUV;
    //     mesh.triangles = new int[]
    //     { 0, 1, 2,
    //         0, 2, 3
    //     };
    // }
}
