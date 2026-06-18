using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PuzzleGoiaba : MonoBehaviour
{
    public GameObject goiabacenario;
    public GameObject GravetoUI;
    public GameObject GoiabaUI;
    public GameObject Objetivo;
    DialogoManager controller;
    int graveto;
    bool goiaba = false;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
        graveto = 0;
    }
    private void Update()
    {
        if (goiaba == true)
        {
            Objetivo.SetActive(false);
        }
    }
    public void InteragirGraveto()
    {
        graveto = 1;
    }
    public void interagirarvore()
    {
        if (graveto == 0)
        {
            controller.Iniciar(1);
        }
        else
        {
            goiabacenario.SetActive(false);
            goiaba = true;
            GravetoUI.SetActive(false);
            GoiabaUI.SetActive(true);
            controller.Iniciar(2);
            controller.Sodialogo = true;
        }
    }
    public void conferir()
    {
        if (controller.Sodialogo == false)
        {
        if (controller.dialog.nextDialog[0] == null)
        {
            Objetivo.SetActive(true);
        }
        }
    }
}
