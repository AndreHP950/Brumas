using UnityEngine;

public class PuzzleInicial : MonoBehaviour
{
    DialogoManager controller;
    int chaveObtida;
    public GameObject chave;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
        chaveObtida = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AbrirPorta()
    {
        if (chaveObtida == 0)
        {
            controller.Iniciar(5);
        }
        else
        {
            chave.SetActive(false);
            controller.Sodialogo = true;
            controller.Iniciar(6);
        }
    }
    public void ColetarChave()
    {
        if (chaveObtida == 0)
        {
            controller.Iniciar(3);
            chaveObtida = 1;
            chave.SetActive(true);
        }
        else
        {
            controller.Iniciar(4);
        }
    }
}
