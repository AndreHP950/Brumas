using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogosManager : MonoBehaviour
{
    public TMP_Text texto;
    public DialogoBase dialog;
    public DialogoBase[] iniciar;

    private void Start()
    {
        texto.text = dialog.text;
    }
    public void Iniciar(int dialogo)
    {
        dialog = iniciar[dialogo];
        texto.text = dialog.text;
    }
    public void Proximo(int dialogo)
    {
        dialog = dialog.nextDialog[dialogo];
        texto.text = dialog.text;
    }
}
