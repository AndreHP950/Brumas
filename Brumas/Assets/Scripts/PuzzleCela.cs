using System;
using UnityEngine;

public class PuzzleCela : MonoBehaviour
{
    DialogoManager controller;
    public bool cela;
    public GameObject celaMula;
    public GameObject celaUI;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
        cela = false;
    }
    public void InteragirCela()
    {
            controller.Iniciar(4);
            cela = true;
    }
    public void InteragirMula()
    {
        if (cela == true)
        {
            celaUI.SetActive(false);
            celaMula.SetActive(true);
            controller.Sodialogo = true;
            controller.Iniciar(0);
        }
        else
        {
            controller.Iniciar(2);
        }
    }
}
