using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MudaVolume : MonoBehaviour
{
    private Slider slider;
    public AudioMixer mixer;
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void MudarVolume(int indice)
    {
        if (indice == 0)
        {
            mixer.SetFloat("Mastervol", slider.value);
        }
        if (indice == 1)
        {
            mixer.SetFloat("Efvol", slider.value);
        }
        if (indice == 2)
        {
            mixer.SetFloat("Musicvol", slider.value);
        }

    }
    /*public void SalvarVolume()
    {
        PlayerPrefs.SetFloat("effvol", slider.value);
        PlayerPrefs.SetFloat("musicvol", slider.value);
        PlayerPrefs.Save();
    }*/
}
