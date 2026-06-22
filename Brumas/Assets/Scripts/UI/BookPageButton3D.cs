using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Botão 3D para o livro físico.
/// - Funciona tanto na cena de jogo (DialogoManager) quanto no Menu (MenuLivroController).
/// </summary>
[RequireComponent(typeof(Collider))]
public class BookPageButton3D : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  REGISTRO ESTÁTICO
    // ─────────────────────────────────────────────
    public static readonly List<BookPageButton3D> Todos = new List<BookPageButton3D>();

    // ─────────────────────────────────────────────
    //  TIPO DE AÇÃO
    // ─────────────────────────────────────────────
    public enum TipoAcao { Proximo, Voltar }

    [Header("Ação")]
    public TipoAcao acao = TipoAcao.Proximo;

    // ─────────────────────────────────────────────
    //  VISUAL
    // ─────────────────────────────────────────────
    [Header("Cores")]
    [SerializeField] private Color corBase  = Color.white;
    [SerializeField] private Color corHover = new Color(1f, 0.93f, 0.78f);
    [SerializeField] private Color corClick = new Color(0.7f, 0.7f, 0.7f);

    // ─────────────────────────────────────────────
    //  BOUNCE
    // ─────────────────────────────────────────────
    [Header("Animação de Bounce")]
    [SerializeField] private float escalaMaxima   = 1.35f;
    [SerializeField] private float duracaoSubida  = 0.10f;
    [SerializeField] private float duracaoDescida = 0.12f;

    // ─────────────────────────────────────────────
    //  SOM
    // ─────────────────────────────────────────────
    [Header("Som (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip   somHover;
    [SerializeField] private AudioClip   somClick;

    // ─────────────────────────────────────────────
    //  INTERNALS
    // ─────────────────────────────────────────────
    private SpriteRenderer _sr;
    private Transform      _imagemTransform;
    private Vector3        _escalaOriginal;
    private bool           _hover = false;

    // ─────────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────────
    private void Awake()
    {
        if (!Todos.Contains(this)) Todos.Add(this);

        _sr = GetComponentInChildren<SpriteRenderer>();
        if (_sr == null) { Debug.LogWarning($"[BookPageButton3D] SpriteRenderer não encontrado em '{name}'."); return; }

        _imagemTransform = _sr.transform;
        _escalaOriginal  = _imagemTransform.localScale;
        _sr.color        = corBase;
    }

    private void OnDestroy()
    {
        Todos.Remove(this);
    }

    // ─────────────────────────────────────────────
    //  INTERAÇÃO
    // ─────────────────────────────────────────────
    private void OnMouseEnter()
    {
        _hover = true;
        AplicarCor(corHover);
        TocarSom(somHover);
    }

    private void OnMouseExit()
    {
        _hover = false;
        AplicarCor(corBase);
    }

    private void OnMouseDown()
    {
        AplicarCor(corClick);
    }

    private void OnMouseUpAsButton()
    {
        AplicarCor(_hover ? corHover : corBase);

        // ── Cena de jogo: DialogoManager ─────────────────
        if (DialogoManager.Instance != null)
        {
            // Texto animando → só finaliza, sem som nem bounce
            if (DialogoManager.Instance.animandoTexto)
            {
                if (acao == TipoAcao.Proximo) DialogoManager.Instance.Proximo();
                else                          DialogoManager.Instance.Voltar();
                return;
            }

            // Botões do Canvas desativados (puzzle) → bloqueia silenciosamente
            if (!DialogoManager.Instance.Botoes.activeSelf) return;

            TocarSom(somClick);
            StartCoroutine(AnimacaoBounce());
            if (acao == TipoAcao.Proximo) DialogoManager.Instance.Proximo();
            else                          DialogoManager.Instance.Voltar();
            return;
        }

        // ── Cena de menu: MenuLivroController ────────────
        if (MenuLivroController.Instance != null)
        {
            bool pode = acao == TipoAcao.Proximo
                ? MenuLivroController.Instance.PodeProximo()
                : MenuLivroController.Instance.PodeVoltar();

            if (!pode) return;

            TocarSom(somClick);
            StartCoroutine(AnimacaoBounce());
            if (acao == TipoAcao.Proximo) MenuLivroController.Instance.Proximo();
            else                          MenuLivroController.Instance.Voltar();
            return;
        }

        Debug.LogWarning("[BookPageButton3D] Nenhum manager encontrado (DialogoManager / MenuLivroController).");
    }

    // ─────────────────────────────────────────────
    //  BOUNCE
    // ─────────────────────────────────────────────
    private IEnumerator AnimacaoBounce()
    {
        if (_imagemTransform == null) yield break;

        Vector3 escalaPico = _escalaOriginal * escalaMaxima;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracaoSubida;
            _imagemTransform.localScale = Vector3.Lerp(_escalaOriginal, escalaPico, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracaoDescida;
            _imagemTransform.localScale = Vector3.Lerp(escalaPico, _escalaOriginal, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        _imagemTransform.localScale = _escalaOriginal;
    }

    // ─────────────────────────────────────────────
    //  HELPERS
    // ─────────────────────────────────────────────
    private void AplicarCor(Color cor)
    {
        if (_sr == null) return;
        _sr.color = cor;
    }

    private void TocarSom(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}