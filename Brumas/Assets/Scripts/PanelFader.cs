using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelFader : MonoBehaviour
{
    private bool faded = false;
    public float duration = 2f;
    public void Fade()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(doFade(canvGroup, canvGroup.alpha, faded ? 1 : 0));
        faded = !faded;
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MenuCreditos") Fade();
    }
    public IEnumerator doFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
    }
}
