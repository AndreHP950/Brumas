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
    public bool AlgoAberto;
    public bool Sodialogo;

    float tempoPorLetra = 0.03f;

    private LTDescr tweenAtual;

    bool animandoTexto;
    string textoAtualCompleto;

    private void Start()
    {
        AlgoAberto = true;
        MostrarTextoAnimado(dialog.text);
        if (dialog.nextDialog[0] == null)
        {
            AlgoAberto = false;
            Botoes.SetActive(false);
        }
    }

    public void Iniciar(int dialogo)
    {
        Botoes.SetActive(true);
        Panel.SetActive(true);
        dialog = iniciar[dialogo];
        MostrarTextoAnimado(dialog.text);
        if (dialog.nextDialog[0] == null)
        {
            AlgoAberto = false;
            Botoes.SetActive(false);
        }
    }

    public void Proximo()
    {
        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }

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

    public void Voltar()
    {
        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }

        dialog = dialog.nextDialog[1];
        MostrarTextoAnimado(dialog.text);
    }

    public void TrocarCena(string Cena)
    {
        SceneManager.LoadScene($"{Cena}");
    }

    void MostrarTextoAnimado(string novoTexto)
    {
        if (tweenAtual != null)
        {
            LeanTween.cancel(gameObject);
        }

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
        {
            LeanTween.cancel(gameObject);
        }

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
        {
            originalVertices[i] = vertices[vertexIndex + i];
        }

        float duracao = tempoPorLetra;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float offsetY = Mathf.Sin((tempo / duracao) * Mathf.PI) * 16.67f;

            for (int i = 0; i < 4; i++)
            {
                vertices[vertexIndex + i] =
                    originalVertices[i] + new Vector3(0, offsetY, 0);
            }

            texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return null;
        }
        for (int i = 0; i < 4; i++)
        {
            vertices[vertexIndex + i] = originalVertices[i];
        }

        texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}