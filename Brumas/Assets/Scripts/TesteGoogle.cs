using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TesteGoogle : MonoBehaviour
{
    // Singleton para garantir que só existe uma instância
    public static TesteGoogle Instance { get; private set; }

    private string url = "https://docs.google.com/forms/d/e/1FAIpQLSdINiVVyvpuckTvWS9IA7qYTx0i8IG43rg7Bgk4I2DykBksgA/formResponse";

    // Respostas das cenas específicas
    public string respostaCena1 = "n/a";
    public string respostaCena3 = "n/a";
    public string respostaCena4 = "n/a";
    public string respostaCena7 = "n/a";
    public string respostaCena9 = "n/a";
    public string respostaCena12 = "n/a";
    public string respostaCena13 = "n/a";
    public string respostaCena15 = "n/a";
    public string respostaCena20 = "n/a";

    void Awake()
    {
        // Verifica se já existe uma instância
        if (Instance != null && Instance != this)
        {
            // Copia os valores da instância antiga para esta
            respostaCena1 = Instance.respostaCena1;
            respostaCena3 = Instance.respostaCena3;
            respostaCena4 = Instance.respostaCena4;
            respostaCena7 = Instance.respostaCena7;
            respostaCena9 = Instance.respostaCena9;
            respostaCena12 = Instance.respostaCena12;
            respostaCena13 = Instance.respostaCena13;
            respostaCena15 = Instance.respostaCena15;
            respostaCena20 = Instance.respostaCena20;

            // Destrói a instância antiga
            Destroy(Instance.gameObject);

            // Esta passa a ser a instância principal
            Instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(gameObject);

            Debug.Log("[TesteGoogle] Dados copiados da instância antiga.");
            return;
        }

        Instance = this;

        if (transform.parent != null)
        {
            Debug.Log($"[TesteGoogle] Movendo objeto de '{transform.parent.name}' para a raiz");
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);

        Debug.Log("[TesteGoogle] Objeto persistente criado e movido para DontDestroyOnLoad");
    }

    void OnDestroy()
    {
        // Remove a referência ao ser destruído
        if (Instance == this)
        {
            Debug.Log("[TesteGoogle] Instância sendo destruída");
            Instance = null;
        }
    }

    /// <summary>
    /// Método que deve ser chamado no OnClick do botão.
    /// Coloque o próprio botão no Inspector como parâmetro.
    /// </summary>
    public void SalvarResposta(Button botao)
    {
        // Pega o texto do botão
        string textoResposta = ObterTextoBotao(botao);

        if (string.IsNullOrEmpty(textoResposta))
        {
            Debug.LogWarning("[TesteGoogle] Não foi possível obter o texto do botão!");
            return;
        }

        // Identifica a cena atual
        string nomeCena = SceneManager.GetActiveScene().name;
        int numeroCena = IdentificarNumeroCena(nomeCena);

        // Salva na string correspondente
        SalvarNaCenaCorrespondente(numeroCena, textoResposta);

        Debug.Log($"[TesteGoogle] Cena {numeroCena} → resposta salva: \"{textoResposta}\"");
    }

    /// <summary>
    /// Obtém o texto do botão (suporta TMP_Text e Text legacy)
    /// </summary>
    string ObterTextoBotao(Button btn)
    {
        TMP_Text tmp = btn.GetComponentInChildren<TMP_Text>();
        if (tmp != null) return tmp.text;

        Text legacy = btn.GetComponentInChildren<Text>();
        if (legacy != null) return legacy.text;

        return "";
    }

    /// <summary>
    /// Identifica o número da cena baseado no nome
    /// </summary>
    private int IdentificarNumeroCena(string nomeCena)
    {
        // Se for a cena "Game", é a cena 1
        if (nomeCena == "Game")
        {
            return 1;
        }

        // Para cenas numeradas tipo "501", "512", "520", etc
        if (nomeCena.Length == 3 && nomeCena.StartsWith("5"))
        {
            // Pega os dois últimos dígitos
            string numeroStr = nomeCena.Substring(1);
            int numero;

            if (int.TryParse(numeroStr, out numero))
            {
                return numero;
            }
        }

        Debug.LogWarning($"[TesteGoogle] Não foi possível identificar o número da cena: {nomeCena}");
        return 0;
    }

    /// <summary>
    /// Salva o texto na variável correspondente à cena
    /// </summary>
    private void SalvarNaCenaCorrespondente(int numeroCena, string texto)
    {
        switch (numeroCena)
        {
            case 1:
                respostaCena1 = texto;
                break;
            case 3:
                respostaCena3 = texto;
                break;
            case 4:
                respostaCena4 = texto;
                break;
            case 7:
                respostaCena7 = texto;
                break;
            case 9:
                respostaCena9 = texto;
                break;
            case 12:
                respostaCena12 = texto;
                break;
            case 13:
                respostaCena13 = texto;
                break;
            case 15:
                respostaCena15 = texto;
                break;
            case 20:
                respostaCena20 = texto;
                break;
            default:
                Debug.LogWarning($"[TesteGoogle] Cena {numeroCena} não tem variável de resposta configurada! Texto: \"{texto}\"");
                break;
        }
    }

    /// <summary>
    /// Envia os dados coletados para o Google Forms
    /// </summary>
    public void EnviarDados()
    {
        StartCoroutine(EnviarDadosCoroutine());
    }

    IEnumerator EnviarDadosCoroutine()
    {
        ImprimirTodasRespostas();
        WWWForm form = new WWWForm();
        form.AddField("entry.1400570944", respostaCena1);
        form.AddField("entry.1025152679", respostaCena3);
        form.AddField("entry.1690388609", respostaCena4);
        form.AddField("entry.1257797676", respostaCena7);
        form.AddField("entry.484831130", respostaCena9); 
        form.AddField("entry.208368808", respostaCena12); 
        form.AddField("entry.2074157192", respostaCena13);  
        form.AddField("entry.421536143", respostaCena15);  
        form.AddField("entry.201561213", respostaCena20);  

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
            Debug.Log("[TesteGoogle] Dados enviados com sucesso!");
        else
            Debug.LogError("[TesteGoogle] Erro ao enviar dados: " + www.error);
    }

    /// <summary>
    /// Método auxiliar para imprimir todas as respostas no console
    /// </summary>
    public void ImprimirTodasRespostas()
    {
        Debug.Log("=== RESPOSTAS COLETADAS ===");
        Debug.Log($"Cena 1 (Game): {respostaCena1}");
        Debug.Log($"Cena 3: {respostaCena3}");
        Debug.Log($"Cena 4: {respostaCena4}");
        Debug.Log($"Cena 7: {respostaCena7}");
        Debug.Log($"Cena 9: {respostaCena9}");
        Debug.Log($"Cena 12: {respostaCena12}");
        Debug.Log($"Cena 13: {respostaCena13}");
        Debug.Log($"Cena 15: {respostaCena15}");
        Debug.Log($"Cena 20: {respostaCena20}");
    }
}