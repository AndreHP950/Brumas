using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveNavmesh : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private Camera sceneCamera;

    [Header("Indicador de Destino (opcional)")]
    [SerializeField] private GameObject clickIndicatorPrefab;
    [SerializeField] private float indicatorDuration = 0.6f;

    [Header("Bloqueio de Movimento")]
    [Tooltip("Impede o movimento enquanto algum painel/diálogo estiver aberto")]
    [SerializeField] private bool blockWhileDialogOpen = true;

    private NavMeshAgent agent;
    private DialogoManager dialogoManager;
    private GameObject currentIndicator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (sceneCamera == null)
            sceneCamera = Camera.main;

        if (sceneCamera == null)
            Debug.LogError("[MoveNavmesh] Nenhuma câmera encontrada! Arraste a câmera no Inspector.");

        if (blockWhileDialogOpen)
            dialogoManager = GameObject.FindGameObjectWithTag("Canvas")
                                       ?.GetComponent<DialogoManager>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (IsDialogOpen())
        {
            Debug.Log("[MoveNavmesh] Bloqueado: diálogo aberto.");
            return;
        }

        if (IsPointerOverUI())
        {
            Debug.Log("[MoveNavmesh] Bloqueado: cursor sobre UI.");
            return;
        }

        if (sceneCamera == null) return;

        Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);

        // Tenta primeiro na walkableLayer configurada
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, walkableLayer))
        {
            Debug.Log($"[MoveNavmesh] Destino: {hit.point} | Objeto: {hit.collider.name}");
            MoveToPoint(hit.point);
            ShowIndicator(hit.point);
        }
        else
        {
            // Debug: testa sem filtro de layer para identificar o problema
            if (Physics.Raycast(ray, out RaycastHit hitAny, Mathf.Infinity))
                Debug.LogWarning($"[MoveNavmesh] Raycast acertou '{hitAny.collider.name}' (layer: {LayerMask.LayerToName(hitAny.collider.gameObject.layer)}) mas NÃO está na walkableLayer configurada.");
            else
                Debug.LogWarning("[MoveNavmesh] Raycast não acertou nenhum objeto.");
        }
    }

    public void MoveToPoint(Vector3 destination)
    {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(destination, out navHit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
        else
        {
            Debug.LogWarning("[MoveNavmesh] Ponto clicado não está próximo do NavMesh.");
        }
    }

    private bool IsDialogOpen()
    {
        return blockWhileDialogOpen && dialogoManager != null && dialogoManager.AlgoAberto;
    }

    private bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current != null
            && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void ShowIndicator(Vector3 position)
    {
        if (clickIndicatorPrefab == null) return;

        if (currentIndicator != null)
            Destroy(currentIndicator);

        currentIndicator = Instantiate(clickIndicatorPrefab, position + Vector3.up * 0.01f, Quaternion.identity);
        Destroy(currentIndicator, indicatorDuration);
    }
}