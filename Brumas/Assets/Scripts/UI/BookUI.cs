using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookController : MonoBehaviour
{
    public static BookController Instance { get; private set; }

    [Header("Referencias")]
    public Animator animatorLivro;
    public GameObject livro3D;

    [Header("Render Texture")]
    [Tooltip("SkinnedMeshRenderer da pagina que exibe o texto")]
    public SkinnedMeshRenderer rendererPagina;
    public RenderTexture renderTexture;

    [Header("Configuracoes")]
    [Tooltip("Nome da cena onde a animacao de abrir deve tocar")]
    public string cenaComAnimacaoAbrir = "Menu";

    [Header("Painel de fundo (opcional)")]
    [Tooltip("Arraste aqui o GameObject 'Panel' do canvas do Menu.\n" +
             "Deixe vazio para desativar o Find automatico que causava bugs.")]
    [SerializeField] private GameObject panelFundo;

    public event System.Action OnPaginaViradaProximo;
    public event System.Action OnPaginaViradaVoltar;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Busca automatica do Panel — so executa se nao foi atribuido no inspector
        // e somente na cena do Menu, para nao colidir com objetos de mesmo nome
        // em outras cenas (o que causava o bug dos sliders).
        if (panelFundo == null)
        {
            GameObject encontrado = GameObject.Find("Panel");
            if (encontrado != null)
                panelFundo = encontrado;
        }
    }

    void Start()
    {
        if (rendererPagina != null && renderTexture != null)
            rendererPagina.material.mainTexture = renderTexture;

        if (livro3D != null)
            livro3D.SetActive(false);
    }

    // ─── Livro ───────────────────────────────────────────────────────────
    public void AbrirLivro()
    {
        if (livro3D != null) livro3D.SetActive(true);

        // so desativa o painel de fundo se ele foi explicitamente atribuido
        if (panelFundo != null) panelFundo.SetActive(false);

        animatorLivro.Play("Idle");
    }

    public void FecharLivro()
    {
        animatorLivro.SetTrigger("Fechar");
        StartCoroutine(DesativarAposFechar());
    }

    private IEnumerator DesativarAposFechar()
    {
        yield return new WaitForSeconds(animatorLivro.GetCurrentAnimatorStateInfo(0).length);

        if (livro3D != null) livro3D.SetActive(false);

        // so reativa o painel de fundo se ele foi explicitamente atribuido
        if (panelFundo != null) panelFundo.SetActive(true);
    }

    // ─── Paginas ─────────────────────────────────────────────────────────
    public void VirarProximaPagina() => animatorLivro.SetTrigger("ProximaPagina");
    public void VirarPaginaAnterior() => animatorLivro.SetTrigger("PaginaAnterior");

    // ─── Botoes 3D ───────────────────────────────────────────────────────
    public void SetBotoesAtivos(bool ativo)
    {
        BookPageButton3D[] snapshot = BookPageButton3D.Todos.ToArray();
        foreach (BookPageButton3D botao in snapshot)
            if (botao != null) botao.gameObject.SetActive(ativo);
    }

    // ─── Animation Events ────────────────────────────────────────────────
    public void AnimEvent_TrocaTextoProximo() => OnPaginaViradaProximo?.Invoke();
    public void AnimEvent_TrocaTextoAnterior() => OnPaginaViradaVoltar?.Invoke();
}