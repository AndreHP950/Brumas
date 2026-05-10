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
    float tempoPorLetra = 0.05f;

    private LTDescr tweenAtual;
    private void Start()
    {
        AlgoAberto = true;
        MostrarTextoAnimado(dialog.text);
    }
    public void Iniciar(int dialogo)
    {
        dialog = iniciar[dialogo];
        MostrarTextoAnimado(dialog.text);
    }

    public void Proximo()
    {
        if (dialog.nextDialog[0] == null)
        {
            if (Sodialogo)
            {
                Botoes.SetActive(false);
                UiExtra.SetActive(true);
            }
            else
            {
                AlgoAberto = false;
                Botoes.SetActive(false);
                Panel.SetActive(false);
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
        texto.text = novoTexto;
        texto.maxVisibleCharacters = 0;

        int totalCaracteres = novoTexto.Length;
        float duracao = totalCaracteres * tempoPorLetra;
        tweenAtual = LeanTween.value(gameObject, 0, totalCaracteres, duracao)
            .setEase(LeanTweenType.linear)
            .setOnUpdate((float valor) =>
            {
                int letrasVisiveis = Mathf.FloorToInt(valor);

                // Só executa quando uma nova letra realmente aparece
                if (letrasVisiveis != texto.maxVisibleCharacters)
                {
                    texto.maxVisibleCharacters = letrasVisiveis;

                    // Faz a última letra exibida dar o pulinho
                    if (letrasVisiveis > 0)
                    {
                        StartCoroutine(PularLetra(letrasVisiveis-1));
                    }
                }
            });
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

        float duracao = tempoPorLetra;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float offsetY = Mathf.Sin((tempo / duracao) * Mathf.PI) * 10f;
            for (int i = 0; i < 4; i++)
            {
                vertices[vertexIndex + i] += new Vector3(0, offsetY, 0);
            }

            texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            for (int i = 0; i < 4; i++)
            {
                vertices[vertexIndex + i] -= new Vector3(0, offsetY, 0);
            }

            yield return null;
        }
    }
}
