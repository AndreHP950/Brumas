using UnityEngine;

[CreateAssetMenu(fileName = "DialogoBase", menuName = "Scriptable Objects/DialogoBase")]
public class DialogoBase : ScriptableObject
{
    public string text;
    public DialogoBase[] nextDialog;
}
