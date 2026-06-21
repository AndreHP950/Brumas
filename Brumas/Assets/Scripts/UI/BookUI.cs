using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookController : MonoBehaviour
{
    public static BookController Instance { get; private set; }

    [Header("Referências")]
    public Animator animatorLivro;
    public GameObject livro3D;

    [Header("Render Texture")]
    [Tooltip("SkinnedMeshRenderer da página que exibe o texto")]
    public SkinnedMeshRenderer rendererPagina;
    public RenderTexture renderTexture;

    [Header("Configurações")]
    [Tooltip("Nome da cena onde a animação de abrir deve tocar")]
    public string cenaComAnimacaoAbrir = "Menu";

    public event System.Action OnPaginaViradaProximo;
    public event System.Action OnPaginaViradaVoltar;

    private GameObject _panelOriginal;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _panelOriginal = GameObject.Find("Panel");

        if (_panelOriginal == null)
            Debug.LogWarning("[BookController] Panel original não encontrado. Verifique o nome 'Panel'.");
    }

    void Start()
    {
        if (rendererPagina != null && renderTexture != null)
        {
            Material matPagina = rendererPagina.material;
            matPagina.mainTexture = renderTexture;
        }

        if (livro3D != null)
            livro3D.SetActive(false);
    }

    // ─── Livro ────────────────────────────────────────────────────────────
    public void AbrirLivro()
    {
        if (livro3D != null)
            livro3D.SetActive(true);

        if (_panelOriginal != null)
            _panelOriginal.SetActive(false);

        string cenaAtual = SceneManager.GetActiveScene().name;

        if (cenaAtual == cenaComAnimacaoAbrir)
            animatorLivro.SetTrigger("Abrir");
        else
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

        if (livro3D != null)
            livro3D.SetActive(false);

        if (_panelOriginal != null)
            _panelOriginal.SetActive(true);
    }

    // ─── Páginas ──────────────────────────────────────────────────────────
    public void VirarProximaPagina()
    {
        animatorLivro.SetTrigger("ProximaPagina");
    }

    public void VirarPaginaAnterior()
    {
        animatorLivro.SetTrigger("PaginaAnterior");
    }

    // ─── Botões 3D ────────────────────────────────────────────────────────
    public void SetBotoesAtivos(bool ativo)
    {
        // Copia a lista antes de iterar para evitar modificação durante o loop
        // (SetActive dispara OnDisable/OnEnable que poderiam alterar a lista)
        BookPageButton3D[] snapshot = BookPageButton3D.Todos.ToArray();

        foreach (BookPageButton3D botao in snapshot)
        {
            if (botao == null) continue;
            botao.gameObject.SetActive(ativo);
        }
    }

    // ─── Animation Events ─────────────────────────────────────────────────
    public void AnimEvent_TrocaTextoProximo()
    {
        OnPaginaViradaProximo?.Invoke();
    }

    public void AnimEvent_TrocaTextoAnterior()
    {
        OnPaginaViradaVoltar?.Invoke();
    }
}