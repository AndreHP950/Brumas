using System.Collections;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public static BookController Instance { get; private set; }

    [Header("Referências")]
    public Animator animatorLivro;
    public GameObject livro3D;

    [Header("Render Texture")]
    public Renderer rendererPagina;
    public RenderTexture renderTexture;

    [Header("Configurações")]
    [Tooltip("Tempo em segundos até trocar o texto no meio da virada da página")]
    public float tempoEsperaFlip = 0.3f;

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
        if (rendererPagina != null && renderTexture != null)
            rendererPagina.material.mainTexture = renderTexture;

        if (livro3D != null)
            livro3D.SetActive(false);
    }

    public void AbrirLivro()
    {
        if (livro3D != null)
            livro3D.SetActive(true);

        animatorLivro.SetTrigger("Abrir");
    }

    public void FecharLivro()
    {
        animatorLivro.SetTrigger("Fechar");
        StartCoroutine(DesativarAposFechar());
    }

    private IEnumerator DesativarAposFechar()
    {
        AnimatorStateInfo stateInfo = animatorLivro.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        if (livro3D != null)
            livro3D.SetActive(false);
    }

    public void VirarProximaPagina()
    {
        animatorLivro.SetTrigger("ProximaPagina");
        StartCoroutine(CallbackNoMeioDaAnimacao(OnPaginaViradaProximo));
    }

    public void VirarPaginaAnterior()
    {
        animatorLivro.SetTrigger("PaginaAnterior");
        StartCoroutine(CallbackNoMeioDaAnimacao(OnPaginaViradaVoltar));
    }

    private IEnumerator CallbackNoMeioDaAnimacao(System.Action callback)
    {
        yield return new WaitForSeconds(tempoEsperaFlip);
        callback?.Invoke();
    }
}