using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PuzzleGoiaba : MonoBehaviour
{
    public GameObject goiabacenario;
    public GameObject GravetoUI;
    public GameObject GoiabaUI; 
    DialogoManager controller;
    int graveto;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
        graveto = 0;
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
            GravetoUI.SetActive(false);
            GoiabaUI.SetActive(true);
            controller.Iniciar(2);
            controller.Sodialogo = true;
        }
    }
}
