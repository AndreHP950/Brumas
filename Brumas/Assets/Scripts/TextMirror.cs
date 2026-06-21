using UnityEngine;
using TMPro;

/// <summary>
/// Espelha automaticamente o TMP_Text do Panel para este componente.
/// Busca a referência pelo nome "Panel" na cena, sem necessidade de arrastar manualmente.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TextMirror : MonoBehaviour
{
    private TMP_Text destino;
    private TMP_Text fonte;

    void Awake()
    {
        destino = GetComponent<TMP_Text>();
        BuscarFonte();
    }

    void BuscarFonte()
    {
        // Busca o GameObject chamado "Panel" na cena
        GameObject panel = GameObject.Find("Panel");

        if (panel == null)
        {
            Debug.LogWarning("[TextMirror] Objeto 'Panel' não encontrado na cena.");
            return;
        }

        // Busca o TMP_Text dentro do Panel (filho direto ou indireto)
        fonte = panel.GetComponentInChildren<TMP_Text>();

        if (fonte == null)
        {
            Debug.LogWarning("[TextMirror] TMP_Text não encontrado dentro do 'Panel'.");
            return;
        }

        // Evita que o destino aponte para si mesmo
        if (fonte == destino)
        {
            Debug.LogWarning("[TextMirror] Fonte e destino são o mesmo objeto. Verifique a hierarquia.");
            fonte = null;
            return;
        }

        Debug.Log($"[TextMirror] Fonte encontrada: '{fonte.gameObject.name}'");
    }

    void LateUpdate()
    {
        if (fonte == null || destino == null) return;

        // Sincroniza o texto
        if (destino.text != fonte.text)
            destino.text = fonte.text;

        // Sincroniza a animação de digitação letra a letra
        if (destino.maxVisibleCharacters != fonte.maxVisibleCharacters)
            destino.maxVisibleCharacters = fonte.maxVisibleCharacters;
    }
}