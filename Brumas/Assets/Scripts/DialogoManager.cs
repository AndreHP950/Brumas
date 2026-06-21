using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DialogoManager : MonoBehaviour
{
    public TMP_Text texto;
    public DialogoBase dialog;
    public DialogoBase[] iniciar;
    public GameObject Botoes;
    public GameObject UiExtra;
    public GameObject Panel;
    public GameObject FadePanel;

    public bool AlgoAberto;
    public bool Sodialogo;

    AudioManager controller;
    PanelFader _fade;
    public float color;

    float tempoPorLetra = 0.03f;

    private LTDescr tweenAtual;

    bool animandoTexto;
    string textoAtualCompleto;

    bool virandoPagina = false;

    public void Start()
    {
        FadePanel.SetActive(true);
        _fade = FadePanel.GetComponent<PanelFader>();
        _fade.Fade();
        controller = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if (BookController.Instance != null)
        {
            // Esconde apenas o visual do Panel, mantendo Botoes visíveis
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
            Botoes.SetActive(false);
            if (Sodialogo)
            {
                Botoes.SetActive(false);
                UiExtra.SetActive(true);
            }
        }
    }

    void OnDestroy()
    {
        if (BookController.Instance != null)
        {
            BookController.Instance.OnPaginaViradaProximo -= AplicarProximoTexto;
            BookController.Instance.OnPaginaViradaVoltar -= AplicarVoltarTexto;
        }
    }

    /// <summary>
    /// Esconde apenas a imagem de fundo do Panel e o texto,
    /// mantendo os Botoes (Voltar/Proximo) visíveis para reposicionar.
    /// </summary>
    void EsconderVisualPanel()
    {
        if (Panel == null) return;

        // Esconde o fundo do Panel
        Image imgPanel = Panel.GetComponent<Image>();
        if (imgPanel != null)
            imgPanel.color = new Color(0, 0, 0, 0);

        // Esconde o texto original (o TextMirror mostra no livro)
        if (texto != null)
            texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 0f);
    }

    public void Iniciar(int dialogo)
    {
        Botoes.SetActive(true);
        Panel.SetActive(true);
        dialog = iniciar[dialogo];

        if (BookController.Instance != null)
            BookController.Instance.AbrirLivro();

        MostrarTextoAnimado(dialog.text);

        if (dialog.nextDialog[0] == null)
        {
            AlgoAberto = false;
            Botoes.SetActive(false);
        }
    }

    public void Proximo()
    {
        controller.PlaySoundButton(0, 1f);

        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }

        if (virandoPagina) return;
        virandoPagina = true;

        if (BookController.Instance != null)
            BookController.Instance.VirarProximaPagina();
        else
            AplicarProximoTexto();
    }

    public void Voltar()
    {
        controller.PlaySoundButton(0, 1f);

        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }

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
            if (Sodialogo)
            {
                dialog = dialog.nextDialog[0];
                MostrarTextoAnimado(dialog.text);
                Botoes.SetActive(false);
                UiExtra.SetActive(true);
            }
            else
            {
                dialog = dialog.nextDialog[0];
                MostrarTextoAnimado(dialog.text);
                AlgoAberto = false;
                Botoes.SetActive(false);
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

        _fade.Fade();
        SceneManager.LoadScene($"{Cena}");
    }

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

        if (index >= textInfo.characterCount)
            yield break;

        if (!textInfo.characterInfo[index].isVisible)
            yield break;

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
}