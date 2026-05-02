using UnityEngine;

public class PuzzleInicial : MonoBehaviour
{
    DialogoManager controller;
    int chaveObtida;
    public GameObject proximoHistoria;
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
            proximoHistoria.SetActive(true);  
            controller.Iniciar(6);
        }
    }
    public void ColetarChave()
    {
        if (chaveObtida == 0)
        {
            controller.Iniciar(3);
            chaveObtida = 1;
        }
        else
        {
            controller.Iniciar(4);
        }
    }
}
