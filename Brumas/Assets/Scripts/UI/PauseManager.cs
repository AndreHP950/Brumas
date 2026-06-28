using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia o pause in-game.
/// Abre o livro 3D e exibe o painelConfig por cima.
/// Funciona independente do MenuLivroController.
/// </summary>
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    // ─────────────────────────────────────────────
    //  REFERENCIAS
    // ─────────────────────────────────────────────
    [Header("Painel de Config (Canvas original)")]
    [Tooltip("Mesmo painelConfig usado no MenuLivroController")]
    [SerializeField] private GameObject painelConfig;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject PauseButton;

    [SerializeField] private GameObject Book;

    [Header("Cena do Menu Principal")]
    [SerializeField] private string cenaMenu = "Menu";


    [Header("Input — tecla de pause")]
    [SerializeField] private KeyCode teclaPause = KeyCode.Escape;

    [Header("Som (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip somAbrirFechar;

    // ─────────────────────────────────────────────
    //  ESTADO
    // ─────────────────────────────────────────────
    private bool _pausado = false;
    public bool Pausado => _pausado;

    // ─────────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        // garante que o jogo nao fica travado se o objeto for destruido pausado
        Time.timeScale = 1f;
    }

    private void Start()
    {
        // garante painel fechado ao iniciar
        if (painelConfig != null) painelConfig.SetActive(false);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(painelConfig);
        DontDestroyOnLoad(Canvas);
        painelConfig.transform.SetParent(Canvas.transform, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(teclaPause))
            AlternarPause();
    }
    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name != "Menu") PauseButton.SetActive(true);
    }

    // ─────────────────────────────────────────────
    //  API PUBLICA
    // ─────────────────────────────────────────────

    /// <summary>Alterna entre pausado e jogando. Conecte ao botao de pause da HUD.</summary>
    public void AlternarPause()
    {
        if (_pausado) Retomar();
        else Pausar();
    }

    /// <summary>Pausa o jogo e abre o painel de config sobre o livro.</summary>
    public void Pausar()
    {
        if (_pausado) return;

        _pausado = true;
        Time.timeScale = 0f;

        TocarSom();

        // abre o livro 3D como fundo visual, sem navegacao de paginas
        if (Book != null)
        {
            Book.SetActive(true);
        }

        // exibe o painel de config por cima
        if (painelConfig != null) painelConfig.SetActive(true);
    }

    /// <summary>Retoma o jogo e fecha tudo.</summary>
    public void Retomar()
    {
        if (!_pausado) return;

        _pausado = false;
        Time.timeScale = 1f;

        TocarSom();

        if (painelConfig != null) painelConfig.SetActive(false);

        if (Book != null)
        {
            Book.SetActive(false);
        }
    }

    /// <summary>Volta ao menu principal. Conecte ao botao "Voltar ao Menu".</summary>
    public void VoltarAoMenu()
    {
        // restaura timeScale antes de trocar de cena
        Time.timeScale = 1f;
        _pausado = false;

        if (painelConfig != null) painelConfig.SetActive(false);

        SceneManager.LoadScene(cenaMenu);
    }

    // ─────────────────────────────────────────────
    //  UTILS
    // ─────────────────────────────────────────────
    private void TocarSom()
    {
        if (audioSource != null && somAbrirFechar != null)
            audioSource.PlayOneShot(somAbrirFechar);
    }
}