using System.Collections;
using UnityEngine;

/// <summary>
/// Animacao de chegada da camera no inicio das cenas de jogo.
/// Coloque este script diretamente na Main Camera.
///
/// A camera parte de uma pose inicial (posicao + rotacao + FOV)
/// e anima ate a pose final (a propria transform da camera no editor).
/// </summary>
public class CameraIntroAnimation : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  CONDICAO DE EXECUCAO
    // ─────────────────────────────────────────────
    [Header("Condicao de Execucao")]
    [Tooltip("Prefixo do nome das cenas onde a animacao deve tocar (ex: '5' roda em CenaQuarto5, 5_Sala, etc.)")]
    [SerializeField] private string prefixoCena = "5";

    [Tooltip("Se true, ignora o prefixo e SEMPRE toca a animacao — util para testar no editor")]
    [SerializeField] private bool forcarAnimacao = false;

    // ─────────────────────────────────────────────
    //  DELAY
    // ─────────────────────────────────────────────
    [Header("Timing")]
    [Tooltip("Tempo de espera (segundos) antes de comecar a animacao")]
    [SerializeField] private float delayInicio = 0.3f;

    [Tooltip("Duracao total da animacao em segundos")]
    [SerializeField] private float duracao = 1.8f;

    // ─────────────────────────────────────────────
    //  POSE INICIAL
    // ─────────────────────────────────────────────
    [Header("Pose Inicial")]
    [Tooltip("Transform vazio que define de onde a camera parte.\n" +
             "Crie um GameObject vazio, posicione/rotacione como quiser e arraste aqui.\n" +
             "Se deixar vazio, usa os valores manuais abaixo.")]
    [SerializeField] private Transform posicaoInicial;

    [Tooltip("Usado SOMENTE se Posicao Inicial estiver vazio")]
    [SerializeField] private Vector3 offsetPosicaoInicial = new Vector3(0f, 8f, -6f);

    [Tooltip("Usado SOMENTE se Posicao Inicial estiver vazio")]
    [SerializeField] private Vector3 rotacaoEulerInicial = new Vector3(55f, 0f, 0f);

    [Tooltip("FOV inicial da camera (Camera Perspectiva). 0 = usar o FOV atual sem alterar")]
    [SerializeField] private float fovInicial = 75f;

    // ─────────────────────────────────────────────
    //  CURVA E ESTILO
    // ─────────────────────────────────────────────
    [Header("Curva de Animacao")]
    [Tooltip("Ease personalizado — o padrao ja da um SmoothStep elegante")]
    [SerializeField] private AnimationCurve curva = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Opcoes Extras")]
    [Tooltip("Bloqueia input do player durante a animacao chamando PlayerController.Instance?.BloquearInput(true)")]
    [SerializeField] private bool bloquearPlayerDuranteAnimacao = true;

    [Tooltip("Evento chamado ao terminar a animacao (opcional)")]
    [SerializeField] private UnityEngine.Events.UnityEvent aoTerminar;

    // ─────────────────────────────────────────────
    //  INTERNALS
    // ─────────────────────────────────────────────
    private Camera _cam;
    private Vector3 _poseFinal;
    private Quaternion _rotFinal;
    private float _fovFinal;

    // ─────────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────────
    private void Awake()
    {
        _cam = GetComponent<Camera>();
        if (_cam == null) _cam = Camera.main;

        // guarda a pose final (posicao atual da camera no editor = destino)
        _poseFinal = transform.position;
        _rotFinal = transform.rotation;
        _fovFinal = _cam != null ? _cam.fieldOfView : 60f;
    }

    private void Start()
    {
        if (!DeveTocar()) return;

        // aplica pose inicial imediatamente para nao mostrar o frame final
        AplicarPoseInicial();

        StartCoroutine(CorAnimacao());
    }

    // ─────────────────────────────────────────────
    //  LOGICA PRINCIPAL
    // ─────────────────────────────────────────────
    private bool DeveTocar()
    {
        if (forcarAnimacao) return true;

        string nomeCena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return nomeCena.StartsWith(prefixoCena, System.StringComparison.OrdinalIgnoreCase);
    }

    private void AplicarPoseInicial()
    {
        if (posicaoInicial != null)
        {
            transform.position = posicaoInicial.position;
            transform.rotation = posicaoInicial.rotation;
        }
        else
        {
            // usa a pose final como base e aplica o offset relativo
            transform.position = _poseFinal + offsetPosicaoInicial;
            transform.rotation = Quaternion.Euler(rotacaoEulerInicial);
        }

        if (_cam != null && fovInicial > 0f)
            _cam.fieldOfView = fovInicial;
    }

    private IEnumerator CorAnimacao()
    {
        // bloqueia input
        if (bloquearPlayerDuranteAnimacao)
            NotificarPlayer(true);

        // delay inicial
        if (delayInicio > 0f)
            yield return new WaitForSeconds(delayInicio);

        // snapshot da pose inicial (pode ter sido alterada no delay)
        Vector3 posIni = transform.position;
        Quaternion rotIni = transform.rotation;
        float fovIni = _cam != null ? _cam.fieldOfView : _fovFinal;

        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = Mathf.Clamp01(tempo / duracao);
            float ease = curva.Evaluate(t);

            transform.position = Vector3.Lerp(posIni, _poseFinal, ease);
            transform.rotation = Quaternion.Slerp(rotIni, _rotFinal, ease);

            if (_cam != null && fovInicial > 0f)
                _cam.fieldOfView = Mathf.Lerp(fovIni, _fovFinal, ease);

            yield return null;
        }

        // garante pose exata ao final
        transform.position = _poseFinal;
        transform.rotation = _rotFinal;
        if (_cam != null && fovInicial > 0f)
            _cam.fieldOfView = _fovFinal;

        // libera input
        if (bloquearPlayerDuranteAnimacao)
            NotificarPlayer(false);

        aoTerminar?.Invoke();
    }

    // ─────────────────────────────────────────────
    //  INTEGRACAO COM PLAYER (adapte ao seu projeto)
    // ─────────────────────────────────────────────
    /// <summary>
    /// Adapte este metodo ao seu PlayerController.
    /// Por padrao tenta chamar SetActive no primeiro objeto com tag "Player".
    /// </summary>
    private void NotificarPlayer(bool bloquear)
    {
        // exemplo genererico: desativa/ativa o componente de movimento do player
        // substitua pela chamada real do seu projeto, ex:
        // PlayerController.Instance?.SetMovimentoAtivo(!bloquear);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // tenta desabilitar um MonoBehaviour chamado "PlayerController" ou similar
        MonoBehaviour movimento = player.GetComponent<MonoBehaviour>();
        if (movimento != null) movimento.enabled = !bloquear;
    }

    // ─────────────────────────────────────────────
    //  GIZMOS — visualiza a pose inicial no editor
    // ─────────────────────────────────────────────
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // pose final (posicao atual)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.4f, "Pose Final");

        // pose inicial
        Vector3 posIni;
        Quaternion rotIni;

        if (posicaoInicial != null)
        {
            posIni = posicaoInicial.position;
            rotIni = posicaoInicial.rotation;
        }
        else
        {
            posIni = transform.position + offsetPosicaoInicial;
            rotIni = Quaternion.Euler(rotacaoEulerInicial);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(posIni, 0.25f);
        UnityEditor.Handles.Label(posIni + Vector3.up * 0.4f, "Pose Inicial");

        // linha entre as duas poses
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(posIni, transform.position);

        // frustum da pose inicial
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            Matrix4x4 matrizOriginal = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(posIni, rotIni, Vector3.one);
            float fov = fovInicial > 0f ? fovInicial : cam.fieldOfView;
            Gizmos.DrawFrustum(Vector3.zero, fov, cam.farClipPlane * 0.15f, cam.nearClipPlane, cam.aspect);
            Gizmos.matrix = matrizOriginal;
        }
    }
#endif
}