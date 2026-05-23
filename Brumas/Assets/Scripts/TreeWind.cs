using UnityEngine;

public class TreeWind : MonoBehaviour
{
    [Header("Referência")]
    [Tooltip("Transform das folhas. Se vazio, busca automaticamente qualquer filho que comece com 'Leaves' e termine com '_LOD0'.")]
    public Transform leavesTransform;

    [Header("Vento - Rotação")]
    [Tooltip("Intensidade máxima do balanço em graus")]
    public float swayAmount = 0.3f;
    [Tooltip("Velocidade do balanço")]
    public float swaySpeed = 0.4f;
    [Tooltip("Intensidade de rajadas aleatórias")]
    public float gustStrength = 0.15f;
    [Tooltip("Frequência das rajadas")]
    public float gustSpeed = 0.1f;

    [Header("Vento - Escala (respiração)")]
    [Tooltip("Variação sutil na escala para simular volume das folhas")]
    public float scaleAmount = 0.005f;
    [Tooltip("Velocidade da variação de escala")]
    public float scaleSpeed = 0.3f;

    // Offsets aleatórios para cada árvore parecer diferente
    private float _offsetX;
    private float _offsetZ;
    private float _offsetScale;
    private Vector3 _originalLocalEuler;
    private Vector3 _originalLocalScale;

    void Start()
    {
        if (leavesTransform == null)
        {
            //leavesTransform = FindLeavesLOD0(transform);
            leavesTransform = transform;
        }

        if (leavesTransform == null)
        {
            Debug.LogWarning($"[TreeWind] Nenhum 'Leaves*_LOD0' encontrado em {gameObject.name}. Defina manualmente ou desative o script.");
            enabled = false;
            return;
        }

        _originalLocalEuler = leavesTransform.localEulerAngles;
        _originalLocalScale = leavesTransform.localScale;

        _offsetX = Random.Range(0f, 100f);
        _offsetZ = Random.Range(0f, 100f);
        _offsetScale = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (leavesTransform == null) return;

        float time = Time.time;

        // --- Balanço principal (Perlin Noise suave) ---
        float swayX = (Mathf.PerlinNoise(time * swaySpeed + _offsetX, 0f) - 0.5f) * 2f * swayAmount;
        float swayZ = (Mathf.PerlinNoise(0f, time * swaySpeed + _offsetZ) - 0.5f) * 2f * swayAmount;

        // --- Rajadas (Perlin Noise mais lento e forte) ---
        float gustX = (Mathf.PerlinNoise(time * gustSpeed + _offsetX + 50f, 10f) - 0.5f) * 2f * gustStrength;
        float gustZ = (Mathf.PerlinNoise(10f, time * gustSpeed + _offsetZ + 50f) - 0.5f) * 2f * gustStrength;

        float totalX = swayX + gustX;
        float totalZ = swayZ + gustZ;

        leavesTransform.localRotation = Quaternion.Euler(
            _originalLocalEuler.x + totalX,
            _originalLocalEuler.y,
            _originalLocalEuler.z + totalZ
        );

        // --- Variação de escala (efeito de "respiração" das folhas) ---
        float scaleNoise = (Mathf.PerlinNoise(time * scaleSpeed + _offsetScale, 20f) - 0.5f) * 2f * scaleAmount;
        leavesTransform.localScale = _originalLocalScale + Vector3.one * scaleNoise;
    }

    /// <summary>
    /// Busca recursiva por qualquer filho cujo nome comece com "Leaves" e termine com "_LOD0".
    /// Funciona para LeavesTreePine_LOD0, LeavesTreeOak_LOD0, etc.
    /// </summary>
    private Transform FindLeavesLOD0(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name.StartsWith("Leaves") && child.name.EndsWith("_LOD0"))
                return child;

            Transform found = FindLeavesLOD0(child);
            if (found != null)
                return found;
        }
        return null;
    }
}