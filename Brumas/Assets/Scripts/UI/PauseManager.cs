using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject painelConfig;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject Book;
    [SerializeField] private GameObject botaoRetomar;
    [SerializeField] private GameObject botaoExit;

    [Header("Cena do Menu Principal")]
    [SerializeField] private string cenaMenu = "Menu";

    [Tooltip("Cenas onde o pause e o botao ficam completamente desativados (alem do Menu)")]
    [SerializeField] private string[] cenasSemPause = { "MenuCreditos" };

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
        Time.timeScale = 1f;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(Canvas);

        if (painelConfig != null) painelConfig.SetActive(false);

        if (botaoRetomar == null)
        {
            Transform t = transform.Find("Despause");
            if (t != null) botaoRetomar = t.gameObject;
        }

        if (botaoExit == null)
        {
            Transform t = transform.Find("Exit");
            if (t != null) botaoExit = t.gameObject;
        }
    }

    /// <summary>
    /// Chame no OnClick do botao de trocar de cena para recolocar
    /// o painelConfig como filho do Canvas antes da mudanca de cena.
    /// </summary>
    public void LimpaPai()
    {
        if (painelConfig != null && Canvas != null)
            painelConfig.transform.SetParent(Canvas.transform, true);
    }

    private bool CenaPermitePause()
    {
        string cena = SceneManager.GetActiveScene().name;
        if (cena == cenaMenu) return false;
        foreach (string bloqueada in cenasSemPause)
            if (cena == bloqueada) return false;
        return true;
    }

    private void Update()
    {
        if (CenaPermitePause() && Input.GetKeyDown(teclaPause))
            AlternarPause();
    }

    private void FixedUpdate()
    {
        if (PauseButton == null) return;
        PauseButton.SetActive(CenaPermitePause() && !_pausado);
    }

    // ─────────────────────────────────────────────
    //  API PUBLICA
    // ─────────────────────────────────────────────
    public void AlternarPause()
    {
        if (_pausado) Retomar();
        else Pausar();
    }

    public void Pausar()
    {
        if (_pausado) return;

        _pausado = true;
        Time.timeScale = 0f;

        TocarSom();

        if (PauseButton != null) PauseButton.SetActive(false);
        if (Book != null) Book.SetActive(true);
        if (painelConfig != null) painelConfig.SetActive(true);
        if (botaoRetomar != null) botaoRetomar.SetActive(true);
        if (botaoExit != null) botaoExit.SetActive(true);
    }

    public void Retomar()
    {
        if (!_pausado) return;

        _pausado = false;
        Time.timeScale = 1f;

        TocarSom();

        if (painelConfig != null) painelConfig.SetActive(false);
        if (Book != null) Book.SetActive(false);
        if (botaoRetomar != null) botaoRetomar.SetActive(false);
        if (botaoExit != null) botaoExit.SetActive(false);
        if (PauseButton != null) PauseButton.SetActive(true);
    }

    public void VoltarAoMenu()
    {
        Time.timeScale = 1f;
        _pausado = false;

        if (painelConfig != null) painelConfig.SetActive(false);
        if (Book != null) Book.SetActive(false);
        if (botaoRetomar != null) botaoRetomar.SetActive(false);
        if (botaoExit != null) botaoExit.SetActive(false);
        if (PauseButton != null) PauseButton.SetActive(false);

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