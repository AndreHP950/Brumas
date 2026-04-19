using Unity.VisualScripting;
using UnityEngine;

public class ObjetoClicavel : MonoBehaviour
{
    public GameObject UI;
    DialogoManager controller;
    void Start()
    {
        Debug.Log("ta funfando");
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
    }
    void Update()
    {
    }
    void OnMouseDown()
    {
        Debug.Log("funfou");
        if (controller.AlgoAberto == false)
        {
            UI.SetActive(true);
            controller.AlgoAberto = true;
        }
    }
}
