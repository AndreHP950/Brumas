using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuLivroController : MonoBehaviour
{
    public static MenuLivroController Instance { get; private set; }

    public enum ModoLivro { Nenhum, Historias, Config, Creditos }

    // ─────────────────────────────────────────────
    //  PAINÉIS DO CANVAS
    // ─────────────────────────────────────────────
    [Header("Painéis do Canvas original")]
    [SerializeField] private GameObject painelConfig;
    [SerializeField] private GameObject painelCreditos;

    // ─────────────────────────────────────────────
    //  HISTÓRIAS — 2 painéis que você monta no editor
    // ─────────────────────────────────────────────
    [Header("Histórias — Painéis (monte você mesmo no editor)")]
    [Tooltip("Painel exibido na página 0 — Introdução do jogo")]
    [SerializeField] private GameObject painelIntro;

    [Tooltip("Painel exibido na página 1 — Descrição do Benicio.")]
    [SerializeField] private GameObject painelBenicio;

    [Tooltip("Botão que aparece só na página do Benicio para iniciar o jogo")]
    [SerializeField] private GameObject botaoIniciarJogo;

    [Tooltip("Nome da cena a carregar ao clicar em Iniciar Jogo")]
    [SerializeField] private string cenaJogo = "Game";

    // ─────────────────────────────────────────────
    //  CRÉDITOS
    // ─────────────────────────────────────────────
    [Header("Créditos — Texto na Render Texture")]
    [SerializeField] private TMP_Text textoCreditosRT;

    [Tooltip("Arquivo .txt com o texto completo (tem prioridade sobre o campo manual)")]
    [SerializeField] private TextAsset arquivoCreditos;

    [TextArea(5, 20)]
    [SerializeField] private string textoCreditosManual;

    [Tooltip("Máx. de caracteres por página — ajuste até o texto ficar equilibrado")]
    [SerializeField] private int maxCharsPorPagina = 600;

    [Header("Créditos — Auto-Avanço")]
    [SerializeField] private float intervaloAutoAvancar = 7f;

    // ─────────────────────────────────────────────
    //  SOM
    // ─────────────────────────────────────────────
    [Header("Som")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip somPagina;

    // ─────────────────────────────────────────────
    //  ESTADO INTERNO
    // ─────────────────────────────────────────────
    private ModoLivro _modo = ModoLivro.Nenhum;
    private int _paginaAtual;
    private List<string> _paginasCreditos = new List<string>();
    private Coroutine _autoAvancar;
    private bool _virandoPagina;

    // ─────────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (BookController.Instance != null)
        {
            BookController.Instance.OnPaginaViradaProximo += OnProximo;
            BookController.Instance.OnPaginaViradaVoltar += OnVoltar;
        }

        PreparaCreditos();
        DesativarPaineis();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;

        if (BookController.Instance != null)
        {
            BookController.Instance.OnPaginaViradaProximo -= OnProximo;
            BookController.Instance.OnPaginaViradaVoltar -= OnVoltar;
        }
    }

    // ─────────────────────────────────────────────
    //  ABRIR MODOS — conecte aos OnClick dos botões
    // ─────────────────────────────────────────────

    /// <summary>Botão "Iniciar / Histórias" → OnClick</summary>
    public void AbrirHistorias()
    {
        _modo = ModoLivro.Historias;
        _paginaAtual = 0;
        DesativarPaineis();
        BookController.Instance?.AbrirLivro();
        AtualizarPaginaHistorias();
        BookController.Instance?.SetBotoesAtivos(true);
    }

    /// <summary>Botão "Configurações" → OnClick</summary>
    public void AbrirConfig()
    {
        _modo = ModoLivro.Config;
        DesativarPaineis();
        BookController.Instance?.AbrirLivro();
        painelConfig.SetActive(true);
        BookController.Instance?.SetBotoesAtivos(false);
    }

    /// <summary>Botão "Créditos" → OnClick</summary>
    public void AbrirCreditos()
    {
        _modo = ModoLivro.Creditos;
        _paginaAtual = 0;
        DesativarPaineis();
        BookController.Instance?.AbrirLivro();
        painelCreditos.SetActive(true);
        AtualizarPaginaCreditos();
        BookController.Instance?.SetBotoesAtivos(true);
        IniciarAutoAvancar();
    }

    /// <summary>Botão fechar/X → OnClick</summary>
    public void Fechar()
    {
        PararAutoAvancar();
        _modo = ModoLivro.Nenhum;
        DesativarPaineis();
        BookController.Instance?.FecharLivro();
    }

    /// <summary>Chamado pelo OnClick do botaoIniciarJogo</summary>
    public void IniciarJogo()
    {
        SceneManager.LoadScene(cenaJogo);
    }

    // ─────────────────────────────────────────────
    //  NAVEGAÇÃO — chamado pelo BookPageButton3D
    // ─────────────────────────────────────────────
    public void Proximo()
    {
        if (_virandoPagina || !PodeProximo()) return;

        _virandoPagina = true;
        PararAutoAvancar();
        TocarSom();

        if (BookController.Instance != null)
            BookController.Instance.VirarProximaPagina();
        else
            OnProximo();
    }

    public void Voltar()
    {
        if (_virandoPagina || !PodeVoltar()) return;

        _virandoPagina = true;
        PararAutoAvancar();
        TocarSom();

        if (BookController.Instance != null)
            BookController.Instance.VirarPaginaAnterior();
        else
            OnVoltar();
    }

    public bool PodeProximo()
    {
        switch (_modo)
        {
            case ModoLivro.Historias: return _paginaAtual < 1;
            case ModoLivro.Creditos: return _paginaAtual < _paginasCreditos.Count - 1;
            default: return false;
        }
    }

    public bool PodeVoltar() => _paginaAtual > 0;

    // ─────────────────────────────────────────────
    //  CALLBACKS DOS ANIMATION EVENTS
    // ─────────────────────────────────────────────
    private void OnProximo()
    {
        _virandoPagina = false;

        switch (_modo)
        {
            case ModoLivro.Historias:
                _paginaAtual++;
                AtualizarPaginaHistorias();
                break;

            case ModoLivro.Creditos:
                _paginaAtual++;
                AtualizarPaginaCreditos();
                break;
        }
    }

    private void OnVoltar()
    {
        _virandoPagina = false;

        switch (_modo)
        {
            case ModoLivro.Historias:
                _paginaAtual--;
                AtualizarPaginaHistorias();
                break;

            case ModoLivro.Creditos:
                _paginaAtual--;
                AtualizarPaginaCreditos();
                break;
        }
    }

    // ─────────────────────────────────────────────
    //  HISTÓRIAS
    // ─────────────────────────────────────────────
    private void AtualizarPaginaHistorias()
    {
        bool naBenicio = _paginaAtual == 1;

        if (painelIntro   != null) painelIntro.SetActive(!naBenicio);
        if (painelBenicio != null) painelBenicio.SetActive(naBenicio);

        // Botão só visível na página do Benicio
        if (botaoIniciarJogo != null) botaoIniciarJogo.SetActive(naBenicio);
    }

    // ─────────────────────────────────────────────
    //  CRÉDITOS
    // ─────────────────────────────────────────────
    private void PreparaCreditos()
    {
        string fonte = arquivoCreditos != null ? arquivoCreditos.text : textoCreditosManual;
        _paginasCreditos = PaginarTexto(fonte, maxCharsPorPagina);
    }

    private void AtualizarPaginaCreditos()
    {
        if (textoCreditosRT == null || _paginasCreditos.Count == 0) return;
        textoCreditosRT.text = _paginasCreditos[_paginaAtual];
    }

    // ─────────────────────────────────────────────
    //  AUTO-AVANÇO
    // ─────────────────────────────────────────────
    private void IniciarAutoAvancar()
    {
        PararAutoAvancar();
        _autoAvancar = StartCoroutine(CorAutoAvancar());
    }

    private void PararAutoAvancar()
    {
        if (_autoAvancar == null) return;
        StopCoroutine(_autoAvancar);
        _autoAvancar = null;
    }

    private IEnumerator CorAutoAvancar()
    {
        while (_paginaAtual < _paginasCreditos.Count - 1)
        {
            yield return new WaitForSeconds(intervaloAutoAvancar);
            yield return new WaitUntil(() => !_virandoPagina);
            if (_paginaAtual >= _paginasCreditos.Count - 1) yield break;

            _virandoPagina = true;
            TocarSom();

            if (BookController.Instance != null)
                BookController.Instance.VirarProximaPagina();
            else
                OnProximo();
        }
    }

    // ─────────────────────────────────────────────
    //  PAGINAÇÃO — nunca corta no meio de uma linha
    // ─────────────────────────────────────────────
    private List<string> PaginarTexto(string texto, int maxChars)
    {
        var paginas = new List<string>();
        if (string.IsNullOrEmpty(texto)) return paginas;

        string[] linhas = texto.Split('\n');
        var buffer = new StringBuilder();

        foreach (string linha in linhas)
        {
            string linhaComQuebra = linha + "\n";

            if (buffer.Length > 0 && buffer.Length + linhaComQuebra.Length > maxChars)
            {
                paginas.Add(buffer.ToString().TrimEnd('\n'));
                buffer.Clear();
            }

            buffer.Append(linhaComQuebra);
        }

        if (buffer.Length > 0)
            paginas.Add(buffer.ToString().TrimEnd('\n'));

        return paginas;
    }

    // ─────────────────────────────────────────────
    //  UTILS
    // ─────────────────────────────────────────────
    private void DesativarPaineis()
    {
        if (painelIntro != null) painelIntro.SetActive(false);
        if (painelBenicio != null) painelBenicio.SetActive(false);
        if (painelConfig != null) painelConfig.SetActive(false);
        if (painelCreditos != null) painelCreditos.SetActive(false);
    }

    private void TocarSom()
    {
        if (audioSource != null && somPagina != null)
            audioSource.PlayOneShot(somPagina);
    }
}