using Unity.VisualScripting;
using UnityEngine;

public class ObjetoClicavel : MonoBehaviour
{
    public GameObject UI;
    DialogoManager controller;
    MoveNavmesh Mover;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogoManager>();
        Mover = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveNavmesh>();
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
            Mover.MoveToPoint(transform.position);
        }
    }
}
