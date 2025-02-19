using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOptimizer : MonoBehaviour
{
    [Header("Shader 설정")]
    public bool useMobileShader = true; // Mobile Shader 사용 여부
    public bool adjustLOD = true; // LOD 조정 여부

    [Header("LOD 설정")]
    [Range(100, 600)]
    public int lodLevel = 300; // LOD 값 조정 (낮을수록 가벼움)

    private void Start()
    {
        OptimizeMaterials();
        EnableInstancingForAllMaterials();
        StaticBatchingUtility.Combine(gameObject);
    }

    private void OptimizeMaterials()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(); // 모든 Renderer 찾기

        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat == null) continue; // Material이 없는 경우 건너뜀

                // LOD 최적화 적용
                if (adjustLOD)
                {
                    mat.shader.maximumLOD = lodLevel;
                    Debug.Log($"🔹 {renderer.gameObject.name}의 Shader LOD를 {lodLevel}으로 설정");
                }

                // Mobile Shader로 변경
                if (useMobileShader)
                {
                    mat.shader = Shader.Find("Autodesk Interactive"); // Mobile 최적화 셰이더 적용
                    Debug.Log($"🚀 {renderer.gameObject.name}에 Autodesk Interactive 적용");
                }
            }
        }
    }

    private void EnableInstancingForAllMaterials()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>(); // 씬 내 모든 Renderer 찾기

        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat != null && !mat.enableInstancing)
                {
                    mat.enableInstancing = true; // GPU Instancing 활성화
                    Debug.Log($"GPU Instancing 활성화: {mat.name}");
                }
            }
        }
    }
}