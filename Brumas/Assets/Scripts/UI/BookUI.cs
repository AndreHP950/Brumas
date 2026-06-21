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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Aplica a RenderTexture no material da página com texto
        if (rendererPagina != null && renderTexture != null)
        {
            // Cria uma instância do material para não afetar outros objetos
            Material matPagina = rendererPagina.material;
            matPagina.mainTexture = renderTexture;
        }

        if (livro3D != null)
            livro3D.SetActive(false);
    }

    public void AbrirLivro()
    {
        if (livro3D != null)
            livro3D.SetActive(true);

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
    }

    public void VirarProximaPagina()
    {
        animatorLivro.SetTrigger("ProximaPagina");
    }

    public void VirarPaginaAnterior()
    {
        animatorLivro.SetTrigger("PaginaAnterior");
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