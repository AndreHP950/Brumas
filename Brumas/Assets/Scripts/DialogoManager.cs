using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance { get; private set; }

    public TMP_Text texto;
    public DialogoBase dialog;
    public DialogoBase[] iniciar;
    public GameObject Botoes;
    public GameObject UiExtra;
    public GameObject Panel;
    [SerializeField] public GameObject FadePanel;

    public bool AlgoAberto;
    public bool Sodialogo;

    [Header("Tutorial (só ativa na cena 'Game')")]
    [Tooltip("Arraste o GameObject do GIF/animação de tutorial aqui")]
    public GameObject gifTutorial;

    [Tooltip("Segundos após o player andar pela primeira vez para esconder o GIF")]
    [SerializeField] private float tempoAteEsconderGif = 3f;

    AudioManager controller;
    PanelFader _fade;
    public float color;

    float tempoPorLetra = 0.03f;

    private LTDescr tweenAtual;

    public bool animandoTexto;
    string textoAtualCompleto;

    bool virandoPagina = false;
    bool _gifJaDesativado = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        FadePanel.SetActive(true);
        _fade = FadePanel.GetComponent<PanelFader>();
        _fade.Fade();
        controller = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if (gifTutorial != null)
            gifTutorial.SetActive(false);

        if (BookController.Instance != null)
        {
            EsconderVisualPanel();
            BookController.Instance.AbrirLivro();
            BookController.Instance.OnPaginaViradaProximo += AplicarProximoTexto;
            BookController.Instance.OnPaginaViradaVoltar += AplicarVoltarTexto;
        }

        AlgoAberto = true;
        MostrarTextoAnimado(dialog.text);

        if (dialog.nextDialog[0] == null)
        {
            AlgoAberto = false;
            SetBotoes(false);

            if (Sodialogo)
                UiExtra.SetActive(true);
        }
        //if (SceneManager.GetActiveScene().name == "MenuCreditos")
        //{
        //    StartCoroutine(EsperarECarregar());
        //}
    }
    void Update()
    {
        // Cheat: tecla 1 abre a cena de créditos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarCena("MenuCreditos");
        }
    }
    //private IEnumerator EsperarECarregar()
    //{
    //    yield return new WaitForSeconds(136f);
    //    TrocarCena("Menu");
    //}
    void OnDestroy()
    {
        if (Instance == this) Instance = null;

        if (BookController.Instance != null)
        {
            BookController.Instance.OnPaginaViradaProximo -= AplicarProximoTexto;
            BookController.Instance.OnPaginaViradaVoltar -= AplicarVoltarTexto;
        }
    }

    // ─── Helper central de botões ──────────────────────────────────────────
    private void SetBotoes(bool ativo)
    {
        Botoes.SetActive(ativo);
        BookController.Instance?.SetBotoesAtivos(ativo);

        if (gifTutorial != null)
        {
            bool naCenaGame = SceneManager.GetActiveScene().name == "Game";
            bool deveAtivar = !ativo && naCenaGame && !_gifJaDesativado;
            gifTutorial.SetActive(deveAtivar);
        }
    }

    // ─── Tutorial GIF ──────────────────────────────────────────────────────
    public void NotificarPlayerAndou()
    {
        if (gifTutorial == null || !gifTutorial.activeSelf || _gifJaDesativado) return;

        _gifJaDesativado = true;
        StartCoroutine(EsconderGifAposDelay());
    }

    private IEnumerator EsconderGifAposDelay()
    {
        yield return new WaitForSeconds(tempoAteEsconderGif);

        if (gifTutorial != null)
            gifTutorial.SetActive(false);
    }

    // ─── Visual Panel ──────────────────────────────────────────────────────
    void EsconderVisualPanel()
    {
        if (Panel == null) return;

        Image imgPanel = Panel.GetComponent<Image>();
        if (imgPanel != null)
            imgPanel.color = new Color(0, 0, 0, 0);

        if (texto != null)
            texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 0f);
    }

    // ─── Iniciar ───────────────────────────────────────────────────────────
    public void Iniciar(int dialogo)
    {
        SetBotoes(true);
        Panel.SetActive(true);
        dialog = iniciar[dialogo];

        if (BookController.Instance != null)
            BookController.Instance.AbrirLivro();

        MostrarTextoAnimado(dialog.text);

        if (dialog.nextDialog[0] == null)
        {
            AlgoAberto = false;
            SetBotoes(false);
        }
    }

    // ─── Navegação ─────────────────────────────────────────────────────────
    public void Proximo()
    {
        // Texto animando: termina instantaneamente, SEM som, SEM virar página
        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }

        // Som só toca quando de fato vai virar página
        controller.PlaySoundButton(0, 1f);

        if (!Botoes.activeSelf) return;
        if (virandoPagina) return;
        virandoPagina = true;

        if (BookController.Instance != null)
            BookController.Instance.VirarProximaPagina();
        else
            AplicarProximoTexto();
    }
    public void ConferirCenaExtra(string proximacena)
    {
        if (dialog.nextDialog[0] == null)
        {
            TrocarCena(proximacena);
        }
    }

    public void Voltar()
    {
        // Texto animando: termina instantaneamente, SEM som, SEM virar página
        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }

        // Som só toca quando de fato vai virar página
        controller.PlaySoundButton(0, 1f);

        if (!Botoes.activeSelf) return;
        if (virandoPagina) return;
        virandoPagina = true;

        if (BookController.Instance != null)
            BookController.Instance.VirarPaginaAnterior();
        else
            AplicarVoltarTexto();
    }

    private void AplicarProximoTexto()
    {
        virandoPagina = false;

        if (dialog.nextDialog[0].nextDialog[0] == null)
        {
            dialog = dialog.nextDialog[0];
            MostrarTextoAnimado(dialog.text);

            if (Sodialogo)
            {
                SetBotoes(false);
                UiExtra.SetActive(true);
            }
            else
            {
                AlgoAberto = false;
                SetBotoes(false);
            }
        }
        else
        {
            dialog = dialog.nextDialog[0];
            MostrarTextoAnimado(dialog.text);
        }
    }

    private void AplicarVoltarTexto()
    {
        virandoPagina = false;
        dialog = dialog.nextDialog[1];
        MostrarTextoAnimado(dialog.text);
    }

    public void TrocarCena(string Cena)
    {
        if (BookController.Instance != null)
            BookController.Instance.FecharLivro();

        StartCoroutine(IniciarFade(Cena));
    }

    // ─── Texto ─────────────────────────────────────────────────────────────
    void MostrarTextoAnimado(string novoTexto)
    {
        if (tweenAtual != null)
            LeanTween.cancel(gameObject);

        StopAllCoroutines();

        textoAtualCompleto = novoTexto;
        animandoTexto = true;

        texto.text = novoTexto;
        texto.maxVisibleCharacters = 0;

        int totalCaracteres = novoTexto.Length;
        float duracao = totalCaracteres * tempoPorLetra;

        tweenAtual = LeanTween.value(gameObject, 0, totalCaracteres, duracao)
            .setEase(LeanTweenType.linear)
            .setOnUpdate((float valor) =>
            {
                int letrasVisiveis = Mathf.FloorToInt(valor);

                if (letrasVisiveis != texto.maxVisibleCharacters)
                {
                    texto.maxVisibleCharacters = letrasVisiveis;

                    if (letrasVisiveis > 0)
                    {
                        int somAleatorio = Random.Range(0, 3);
                        controller.PlaySoundButton(somAleatorio, 0.2f);
                        StartCoroutine(PularLetra(letrasVisiveis - 1));
                    }
                }
            })
            .setOnComplete(() =>
            {
                animandoTexto = false;
                texto.maxVisibleCharacters = totalCaracteres;
                texto.ForceMeshUpdate();
                texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            });
    }

    void MostrarTextoInstantaneo()
    {
        if (tweenAtual != null)
            LeanTween.cancel(gameObject);

        StopAllCoroutines();

        animandoTexto = false;
        texto.text = textoAtualCompleto;
        texto.maxVisibleCharacters = textoAtualCompleto.Length;
        texto.ForceMeshUpdate();
        texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    public IEnumerator PularLetra(int index)
    {
        texto.ForceMeshUpdate();

        TMP_TextInfo textInfo = texto.textInfo;

        if (index >= textInfo.characterCount) yield break;
        if (!textInfo.characterInfo[index].isVisible) yield break;

        TMP_CharacterInfo charInfo = textInfo.characterInfo[index];
        int materialIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;

        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        Vector3[] originalVertices = new Vector3[4];

        for (int i = 0; i < 4; i++)
            originalVertices[i] = vertices[vertexIndex + i];

        float duracao = tempoPorLetra;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float offsetY = Mathf.Sin((tempo / duracao) * Mathf.PI) * 16.67f;

            for (int i = 0; i < 4; i++)
                vertices[vertexIndex + i] = originalVertices[i] + new Vector3(0, offsetY, 0);

            texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            yield return null;
        }

        for (int i = 0; i < 4; i++)
            vertices[vertexIndex + i] = originalVertices[i];

        texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
    
        public IEnumerator IniciarFade(string Cena)
    {
        yield return new WaitForSeconds(3f);
        _fade.Fade();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene($"{Cena}");
    }
}