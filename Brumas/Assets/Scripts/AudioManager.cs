using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // ─────────────────────────────────────────────
    //  MIXER
    // ─────────────────────────────────────────────
    [SerializeField] private AudioMixer mixer;

    // ─────────────────────────────────────────────
    //  EFEITOS
    // ─────────────────────────────────────────────
    [Header("Efeitos Sonoros")]
    [SerializeField] private AudioClip[] UI;
    [SerializeField] private AudioClip[] Walk;

    // ─────────────────────────────────────────────
    //  MÚSICA
    // ─────────────────────────────────────────────
    [Header("Músicas")]
    [SerializeField] private AudioClip musicaMenu;

    [Tooltip("Músicas aleatórias para cenas normais (501–524)")]
    [SerializeField] private AudioClip[] musicasCenasNormais;

    [Tooltip("Músicas aleatórias para cenas extras")]
    [SerializeField] private AudioClip[] musicasCenasExtras;

    [Tooltip("Música final — tocada apenas ao concluir CenaExtra11. Deixe vazio por enquanto.")]
    [SerializeField] private AudioClip musicaFinal;

    [Header("AudioSource de Música (separado dos efeitos)")]
    [Tooltip("AudioSource com output no grupo 'Musica' do mixer")]
    [SerializeField] private AudioSource audioSourceMusica;

    // ─────────────────────────────────────────────
    //  INTERNALS
    // ─────────────────────────────────────────────
    private AudioSource _audioSourceEfeitos;
    private string _cenaAtual = "";

    // ─────────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Move para a raiz antes de DontDestroyOnLoad (igual ao TesteGoogle)
        // Evita ser destruído junto com o Canvas ao trocar de cena
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _audioSourceEfeitos = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnScenaCarregada;

        // Toca a música da cena inicial
        TratarMusicaDaCena(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        SceneManager.sceneLoaded -= OnScenaCarregada;
    }

    // ─────────────────────────────────────────────
    //  TROCA DE CENA
    // ─────────────────────────────────────────────
    private void OnScenaCarregada(Scene cena, LoadSceneMode modo)
    {
        TratarMusicaDaCena(cena.name);
    }

    private void TratarMusicaDaCena(string nomeCena)
    {
        if (nomeCena == _cenaAtual) return; // mesma cena, não reinicia
        _cenaAtual = nomeCena;

        if (nomeCena == "Menu")
        {
            TocarMusica(musicaMenu);
            return;
        }

        // if (nomeCena == "CenaExtra11")
        //{
        //    TocarMusica(musicaFinal);
        //    return;
        //}

        if (nomeCena.Contains("CenaExtra"))
        {
            TocarMusicaAleatoria(musicasCenasExtras);
            return;
        }

        // Cenas normais (501–524 e qualquer outra que não seja extra/menu)
        TocarMusicaAleatoria(musicasCenasNormais);
    }

    // ─────────────────────────────────────────────
    //  HELPERS DE MÚSICA
    // ─────────────────────────────────────────────
    private void TocarMusica(AudioClip clip)
    {
        if (audioSourceMusica == null || clip == null) return;

        // Não reinicia se já está tocando a mesma música
        if (audioSourceMusica.clip == clip && audioSourceMusica.isPlaying) return;

        audioSourceMusica.clip = clip;
        audioSourceMusica.loop = true;
        audioSourceMusica.Play();
    }

    private void TocarMusicaAleatoria(AudioClip[] lista)
    {
        if (lista == null || lista.Length == 0) return;

        int indice = Random.Range(0, lista.Length);
        TocarMusica(lista[indice]);
    }

    // ─────────────────────────────────────────────
    //  EFEITOS SONOROS (API existente mantida)
    // ─────────────────────────────────────────────
    public static void PlaySound(int i)
    {
        if (Instance == null || Instance.UI == null) return;
        Instance._audioSourceEfeitos.PlayOneShot(Instance.UI[i]);
    }

    public void PlaySoundButton(int i, float volume)
    {
        if (UI == null || i >= UI.Length) return;
        _audioSourceEfeitos.PlayOneShot(UI[i], volume);
    }

    public static void WalkSound(int i)
    {
        if (Instance == null || Instance.Walk == null) return;
        Instance._audioSourceEfeitos.pitch = Random.Range(0.85f, 1.5f);
        Instance._audioSourceEfeitos.PlayOneShot(Instance.Walk[i]);
    }
}