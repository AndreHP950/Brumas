using UnityEngine;

public class AbrirCreditos : MonoBehaviour
{
    public float segundos = 5f;

    private float velocidade;

    private void Start()
    {
        velocidade = 360f / segundos;
    }

    private void Update()
    {
        transform.Rotate(velocidade * Time.deltaTime, 0f, 0f, Space.Self);
    }
}