using System.Collections;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    private bool faded = false;
    public float duration = 0.4f;
    public void Fade()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(doFade(canvGroup, canvGroup.alpha, faded ? 1 : 0));
        faded = !faded;
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
