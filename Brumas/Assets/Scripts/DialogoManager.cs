using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private void Start()
    {
        AlgoAberto = true;
        texto.text = dialog.text;
    }
    private void Update()
    {
    }
    public void Iniciar(int dialogo)
    {
        dialog = iniciar[dialogo];
        texto.text = dialog.text;
    }
    public void Proximo()
    {
        if (dialog.nextDialog[0] == null)
        {
            if (Sodialogo == true)
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
            texto.text = dialog.text;
        }
    }
    public void Voltar()
    {
        dialog = dialog.nextDialog[1];
        texto.text = dialog.text;
    }
    public void TrocarCena(string Cena)
    {
        SceneManager.LoadScene($"{Cena}");
    }
}
