using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

using UnityEngine.Rendering.Universal;

public class RenderObject
{
    public int _renderQueue;
    public Renderer _renderer;
    public Material _material;
    public int _index;

    public RenderObject(int renderQueue, Renderer renderer, Material material, int index)
    {
        this._renderQueue = renderQueue;
        this._renderer = renderer;
        this._material = material;
        this._index = index;
    }


    public static void SortByRenderQueue(List<RenderObject> renderObjectList)
    {
        renderObjectList.Sort((a, b) => { return a.renderQueue.CompareTo(b.renderQueue); });
    }


    public int renderQueue
    {
        get { return _renderQueue; }
    }
    public Renderer renderer
    {
        get { return _renderer; }
    }
    public Material material
    {
        get { return _material; }

    }
    public int index
    {
        get { return _index; }
    }

}

public class TransparentReflection : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_TargetObjectList = new List<GameObject>();
    [SerializeField]
    private List<Renderer> m_InvisibleRenderList = new List<Renderer>();
    [SerializeField]
    private Shader m_GaussBlurShader;
    [Range(0, 1)]
    public float m_BlurSpreadSize;
    [Range(5, 20)]
    //public float DistanceFadeSize = 10;
    [SerializeField]
    private int m_RTSize = 512;

    private bool isReflectionCameraRendering = false;
    List<RenderObject> LinkRenderQueue_Transparent = new List<RenderObject>();
    List<RenderObject> LinkRenderQueue = new List<RenderObject>();
    List<RenderObject> ClothRenderQueue_Transparent = new List<RenderObject>();
    List<RenderObject> ClothRenderQueue = new List<RenderObject>();

    private CommandBuffer m_commandBuffer = null;
    private RenderTexture reflectionRT = null;
    private Material m_RefMat = null;
    private Material m_depthMat;
    private Renderer reflectionRender = null;
    private static int TempRT = Shader.PropertyToID("TempRefRT");



    List<Camera> m_cameras = new List<Camera>();
    #region event
    private void Awake()
    {
        m_depthMat = new Material(m_GaussBlurShader);
        m_depthMat.hideFlags = HideFlags.HideAndDontSave;
        reflectionRT = RenderTexture.GetTemporary(m_RTSize, m_RTSize, 24);
        reflectionRender = gameObject.GetComponent<Renderer>();
        m_commandBuffer = new CommandBuffer();

    }

    private void Start()
    {
        UpdateAllRender();
        m_RefMat = Instantiate(reflectionRender.sharedMaterial);
        reflectionRender.material = m_RefMat;
        RenderPipelineManager.beginCameraRendering += relectionRendering;
    }

    public void OnDestroy()
    {
        RemoveCommandBuffer();
        LinkRenderQueue_Transparent.Clear();
        LinkRenderQueue.Clear();
        ClothRenderQueue_Transparent.Clear();
        ClothRenderQueue.Clear();
        m_TargetObjectList.Clear();
        m_InvisibleRenderList.Clear();
        Cleanup();
        DestroyObject(m_depthMat);
        DestroyObject(m_RefMat);
        RenderTexture.ReleaseTemporary(reflectionRT);
        reflectionRT = null;
        RenderPipelineManager.beginCameraRendering -= relectionRendering;
    }

    private void Update()
    {
        RemoveCommandBuffer();
    }


    private void Cleanup()
    {

        if (m_commandBuffer != null)
            m_commandBuffer.Dispose();
    }
    #endregion

    #region 对外接口
    /// <summary>
    /// 添加镜像目标
    /// </summary>
    /// <param name="target"></param>
    public void AddTarget(GameObject target)
    {
        if (m_TargetObjectList.Contains(target))
            return;

        else
            m_TargetObjectList.Add(target);
    }

    /// <summary>
    /// 移除镜像目标
    /// </summary>
    /// <param name="target"></param>
    public void RemoveTarget(GameObject target)
    {
        if (!m_TargetObjectList.Contains(target))
            return;

        else
            m_TargetObjectList.Remove(target);
    }

    /// <summary>
    /// 过滤镜像人物中不显示的节点,每次刷新后清空
    /// </summary>
    /// <param name="target"></param>
    public void SetInvisibleObject(GameObject invisibleObject)
    {
        Renderer[] invisibleRenderers = invisibleObject.GetComponentsInChildren<Renderer>(true);
        for (int j = 0; j < invisibleRenderers.Length; j++)
        {
            if (m_InvisibleRenderList.Contains(invisibleRenderers[j]))
                continue;
            else
                m_InvisibleRenderList.Add(invisibleRenderers[j]);
        }

    }

    public void RemoveSelection()
    {
        GameObject curObject = GameObject.FindWithTag("ins");
        SetInvisibleObject(curObject);
    }

    /// <summary>
    /// 刷新renderList，每次添加、删除镜像人物，更换人物衣服、挂件后调用
    /// </summary>
    public void UpdateAllRender()
    {
        LinkRenderQueue_Transparent.Clear();
        LinkRenderQueue.Clear();
        ClothRenderQueue_Transparent.Clear();
        ClothRenderQueue.Clear();
        SetInvisibleObject(this.gameObject);

        for (int j = 0; j < m_TargetObjectList.Count; j++)
        {   

            if (m_TargetObjectList[j] == null)
                continue;

            for (int i = 0; i < m_TargetObjectList[j].transform.childCount; i++)
            {
                Transform child = m_TargetObjectList[j].transform.GetChild(i);

                if (child.gameObject.name == "_LinkObjects" || child.gameObject.name == "BPLink")
                {
                    AddRender(child, LinkRenderQueue, LinkRenderQueue_Transparent);
                    continue;
                }
                else
                    AddRender(child, ClothRenderQueue, ClothRenderQueue_Transparent);
            }
        }
        if (LinkRenderQueue_Transparent.Count > 1)
            RenderObject.SortByRenderQueue(LinkRenderQueue_Transparent);
        if (ClothRenderQueue_Transparent.Count > 1)
            RenderObject.SortByRenderQueue(ClothRenderQueue_Transparent);
        m_InvisibleRenderList.Clear();
    }

    public void AddSelection()
    {
        GameObject curObject = GameObject.FindWithTag("Player");
        AddTarget(curObject);
    }
    #endregion

    #region reflection Matrix utility
    Matrix4x4 CaculateReflectMatrix()
    {
        var normal = transform.up;
        var d = -Vector3.Dot(normal, transform.position);
        var reflectM = new Matrix4x4();
        reflectM.m00 = 1 - 2 * normal.x * normal.x;
        reflectM.m01 = -2 * normal.x * normal.y;
        reflectM.m02 = -2 * normal.x * normal.z;
        reflectM.m03 = -2 * d * normal.x;

        reflectM.m10 = -2 * normal.x * normal.y;
        reflectM.m11 = 1 - 2 * normal.y * normal.y;
        reflectM.m12 = -2 * normal.y * normal.z;
        reflectM.m13 = -2 * d * normal.y;

        reflectM.m20 = -2 * normal.x * normal.z;
        reflectM.m21 = -2 * normal.y * normal.z;
        reflectM.m22 = 1 - 2 * normal.z * normal.z;
        reflectM.m23 = -2 * d * normal.z;

        reflectM.m30 = 0;
        reflectM.m31 = 0;
        reflectM.m32 = 0;
        reflectM.m33 = 1;
        return reflectM;
    }

    Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
    {
        var result = projection;
        Vector4 q = result.inverse * new Vector4(
            Mathf.Sign(clipPlane.x),
            Mathf.Sign(clipPlane.y),
            1.0f,
            1.0f
        );
        Vector4 c = clipPlane * (2.0F / (Vector4.Dot(clipPlane, q)));
        // third row = clip plane - fourth row
        result[2] = c.x - result[3];
        result[6] = c.y - result[7];
        result[10] = c.z - result[11];
        result[14] = c.w - result[15];
        return result;
    }

    #endregion

    #region planar reflection

    private void relectionRendering(ScriptableRenderContext context, Camera camera)
    {
        if (reflectionRender == null || m_RefMat == null)
            return;
        if (isReflectionCameraRendering)
            return;
        isReflectionCameraRendering = true;
        configCommandBuffer(context, camera);
        isReflectionCameraRendering = false;
    }

    private void DrawRender(List<RenderObject> RenderQueue)
    {
        var enumerator = RenderQueue.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var pair = enumerator.Current;
            Renderer renderer = pair.renderer;
            if (!renderer || !renderer.gameObject.activeInHierarchy || !renderer.enabled)
                continue;
            if (pair.material == null|| pair.material.shader.name == "DGM/Advanced/Distort_Additive")
                continue;
            int metaIndex = pair.material.FindPass("Meta");
            int surfaceCasterIndex = pair.material.FindPass("SurfaceCaster");
            int selfCasterIndex = pair.material.FindPass("SelfCaster");
            if (metaIndex != -1 || surfaceCasterIndex != -1 || selfCasterIndex != -1)
            {
                int passNum = pair.material.passCount;
                for (int i = 0; i < passNum; i++)
                {
                    if (i == metaIndex || i == surfaceCasterIndex || i == selfCasterIndex)
                        continue;
                    m_commandBuffer.DrawRenderer(renderer, pair.material, pair.index, i);
                }
            }
            else
                m_commandBuffer.DrawRenderer(renderer, pair.material, pair.index);

        }
    }

    private void AddRender(Transform child, List<RenderObject> RenderQueue, List<RenderObject> RenderQueue_Transparent)
    {
        List<Transform> renderTransformList = new List<Transform>();
        var transformList = child.gameObject.GetComponentsInChildren<Transform>(true);
        for (int index = 0; index < transformList.Length; index++)
        {
            if (transformList[index].gameObject.layer == 15)
                continue;
            renderTransformList.Add(transformList[index]);
        }

        List<Renderer> targetRenderers = new List<Renderer>();
        for (int k = 0; k < renderTransformList.Count; k++)
        {
            var curRenderer = renderTransformList[k].gameObject.GetComponent<Renderer>();
            if (curRenderer == null)
                continue;
            targetRenderers.Add(curRenderer);
        }
        //Renderer[] targetRenderers = child.gameObject.GetComponentsInChildren<Renderer>(true);
        for (int j = 0; j < targetRenderers.Count; j++)
        {
            if (m_InvisibleRenderList.Contains(targetRenderers[j]))
                continue;
            var Materials = targetRenderers[j].sharedMaterials;
            if (Materials == null)
                continue;
            for (int i = 0; i < Materials.Length; i++)
            {
                if (Materials[i] == null)
                    continue;
                KeyValuePair<int, Material> temp = new KeyValuePair<int, Material>(i, Materials[i]);
                // 区分透明与非透明
                if (Materials[i].renderQueue <= 2500)
                {
                    RenderObject m_RenderObject = new RenderObject(Materials[i].renderQueue, targetRenderers[j], Materials[i], i);

                    RenderQueue.Add(m_RenderObject);
                }

                else
                {
                    RenderObject m_RenderObject = new RenderObject(Materials[i].renderQueue, targetRenderers[j], Materials[i], i);
                    RenderQueue_Transparent.Add(m_RenderObject);
                }

            }

        }
    }

    private void configCommandBuffer(ScriptableRenderContext context, Camera camera)
    {
        if (camera == null)
            return;
        if (m_commandBuffer != null)
        {
            m_commandBuffer.Clear();
        }
        
        m_commandBuffer.name = "TransparentReflection";
        m_commandBuffer.SetRenderTarget(reflectionRT);
        m_commandBuffer.ClearRenderTarget(true, true, Color.clear);

        var reflectM = CaculateReflectMatrix();

        var normal = transform.up;
        var d = -Vector3.Dot(normal, transform.position);
        var plane = new Vector4(normal.x, normal.y, normal.z, d);
        var reflectionViewMatrix = camera.worldToCameraMatrix * reflectM;
        var viewSpacePlane = reflectionViewMatrix.inverse.transpose * plane;

        Matrix4x4 clipMatrix = CalculateObliqueMatrix(camera.projectionMatrix, viewSpacePlane);

        //设置矩阵
        m_commandBuffer.SetViewMatrix(reflectionViewMatrix);
        m_commandBuffer.SetProjectionMatrix(clipMatrix);

        //渲染反射物体
        m_commandBuffer.SetInvertCulling(true);
        DrawRender(ClothRenderQueue);
        DrawRender(ClothRenderQueue_Transparent);
        DrawRender(LinkRenderQueue);
        DrawRender(LinkRenderQueue_Transparent);
        m_commandBuffer.SetInvertCulling(false);

        //模糊
        if (m_depthMat && m_BlurSpreadSize > 0)
        {
            m_depthMat.SetFloat("_DownSampleValue", m_BlurSpreadSize);
            m_commandBuffer.GetTemporaryRT(TempRT, reflectionRT.width, reflectionRT.height);
            m_commandBuffer.Blit(reflectionRT, TempRT, m_depthMat, 1);
            m_commandBuffer.Blit(TempRT, reflectionRT, m_depthMat, 2);
            m_commandBuffer.ReleaseTemporaryRT(TempRT);
        }
        
        m_RefMat.SetTexture("_ReflectionTex", reflectionRT);
        // m_RefMat.SetFloat("_StartPixel", GetStartPixel(camera));
        
        context.ExecuteCommandBuffer(m_commandBuffer);
        context.Submit();
        
        m_commandBuffer.Clear();
    }
    
    #endregion

    void RemoveCommandBuffer()
    {
        if (null == m_commandBuffer)
        {
            return;
        }
        for (int i = 0; i < m_cameras.Count; i++)
        {
            if (null != m_cameras[i])
            {
                m_cameras[i].RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, m_commandBuffer);
            }
        }
        m_cameras.Clear();
    }
}



