using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PuzzleProcurar : MonoBehaviour
{
    DialogoManager controller;
    public bool objeto1;
    public bool objeto2;
    public bool objeto3;
    public bool objeto4;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
        objeto1 = false;
        objeto2 = false;
        objeto3 = false;
        objeto4 = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void conferir()
    {
            if (objeto1 == true && objeto2 == true && objeto3 == true && objeto4 == true)
            {
                controller.AlgoAberto = true;
                controller.Botoes.SetActive(true);
                controller.Panel.SetActive(true);
                controller.Sodialogo = true;
                controller.Iniciar(8);
                objeto4 = false;
            objeto3 = false;
            }
    }
    //interagir com os objetos
    // 0 = interagir, 1 = observar
    public void Interagir1(int i)
    {
        if (i == 0)
        {
            controller.Iniciar(0);
            objeto1 = true;
            conferir();
        }
        else
        {
            controller.Iniciar(1);
        }
    }
    public void Interagir2(int i)
    {
        if (i == 0)
        {
            controller.Iniciar(2);
            objeto2 = true;
            conferir();
        }
        else
        {
            controller.Iniciar(3);
        }
    }
    public void Interagir3(int i)
    {
        if (i == 0)
        {
            controller.Iniciar(4);
            objeto3 = true;
            conferir();
        }
        else
        {
            controller.Iniciar(5);
        }

    }
    public void Interagir4(int i)
    {
        if (i == 0)
        {
            controller.Iniciar(6);
            objeto4 = true;
            conferir();
        }
        else
        {
            controller.Iniciar(7);
        }

    }
}
