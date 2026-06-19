using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class TesteGoogle : MonoBehaviour
{
    string cenaresposta;
    int index=0;
    private string url = "https://docs.google.com/forms/d/e/1FAIpQLSdINiVVyvpuckTvWS9IA7qYTx0i8IG43rg7Bgk4I2DykBksgA/formResponse";
    void Start()
    {
        StartCoroutine(EnviarDados(cenaresposta, "Resposta 2", "Resposta 3", "não deu a goiaba"));
        atualizarresposta(ref cenaresposta);
    }
    public void atualizarresposta(ref string xablau)
    {
        xablau = "1";
        index++;
    }
    IEnumerator EnviarDados(string resposta1, string resposta2,string resposta3, string resposta4)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1285887200", resposta1); // Substitua pelo ID correto
        form.AddField("entry.1400570944", resposta2); // ID do segundo campo
        form.AddField("entry.1025152679", resposta3); // Substitua pelo ID correto
        form.AddField("entry.1690388609", resposta4);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Dados enviados com sucesso!");
        }
        else
        {
            Debug.Log("Erro ao enviar dados: " + www.error);
        }
    }
}