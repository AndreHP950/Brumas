using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogoManager : MonoBehaviour
{
    public TMP_Text texto;
    public DialogoBase dialog;
    public DialogoBase[] iniciar;
    public int dialogoAtual;
    int chaveObtida;
    public GameObject botaoSair;
    public bool AlgoAberto;
    private void Start()
    {
        AlgoAberto = true;
        chaveObtida = 0;
        texto.text = dialog.text;
    }
    public void Iniciar(int dialogo)
    {
        dialog = iniciar[dialogo];
        texto.text = dialog.text;
        dialogoAtual = 0;
    }
    public void Proximo()
    {
        dialog = dialog.nextDialog[0];
        texto.text = dialog.text;
        dialogoAtual++;
    }
    public void Fechar()
    {
        AlgoAberto = false;
    }
    public void Conferir(int dialogoMax)
    {
        if (dialogoMax == dialogoAtual)
        {
            botaoSair.SetActive(true);
        }
    }
    public void TrocarCena(string Cena)
    {
        SceneManager.LoadScene($"{Cena}");
    }

    // A partir daqui são funções relacionadas aos objetos no geral
    public void AbrirPorta()
    {
        if (chaveObtida == 0) 
        {
            Iniciar(5);
        }
        else
        {
            Iniciar(6);
            botaoSair.SetActive(false);
        }    
    }
    public void ColetarChave()
    {
        if (chaveObtida == 0)
        {
            Iniciar(3);
            chaveObtida = 1;
        }
        else
        {
            Iniciar(4);
        }
    }
}
