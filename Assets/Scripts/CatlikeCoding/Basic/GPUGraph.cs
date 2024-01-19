using UnityEngine;

public class GPUGraph : MonoBehaviour {
    
    [SerializeField, Range(20, 200)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function;
    
    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;
    
    ComputeBuffer positionsBuffer;
    
    [SerializeField]
    ComputeShader computeShader;
    
    static readonly int positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");
    
    void OnEnable () {
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }
    
    void UpdateFunctionOnGPU () {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        
        computeShader.SetBuffer(0, positionsId, positionsBuffer);
        
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
        
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }
    
    void Update () {
        UpdateFunctionOnGPU();
    }
    
    void OnDisable () {
        positionsBuffer.Release();
        positionsBuffer = null;
    }
}