using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Anima��es de bot�o estilo cozy / liter�rio (What Remains of Edith Finch).
/// Adicione ao GameObject do bot�o e configure o preset no Inspector.
/// Para som: arraste os AudioClips nos campos opcionais.
/// </summary>
public class ButtonAnimated : MonoBehaviour
{
    // ?????????????????????????????????????????????
    //  PRESETS
    // ?????????????????????????????????????????????
    public enum ButtonPreset
    {
        /// Sutil � escala m�nima, fade suave. Ideal para bot�es de menu principal.
        Gentle,

        /// Levemente expressivo � pequena rota��o + brilho no texto. Ideal para sele��o de itens.
        Warm,

        /// Quase invis�vel � s� opacidade muda. Ideal para bot�es secund�rios / HUD.
        Whisper,

        /// Aparece como se fosse digitado/escrito. Ideal para bot�es de di�logo.
        Ink
    }

    // ?????????????????????????????????????????????
    //  INSPECTOR
    // ?????????????????????????????????????????????
    [Header("Preset")]
    public ButtonPreset preset = ButtonPreset.Gentle;

    [Header("Refer�ncias (opcional � busca autom�tica se vazio)")]
    [SerializeField] private Image targetImage;
    [SerializeField] private TextMeshProUGUI targetText;

    [Header("Cores do Texto")]
    [SerializeField] private Color textColorNormal  = new Color(0.92f, 0.88f, 0.82f, 1f); // creme
    [SerializeField] private Color textColorHover   = new Color(1f,    0.93f, 0.78f, 1f); // �mbar suave
    [SerializeField] private Color textColorClick   = new Color(1f,    1f,    1f,    1f); // branco puro

    [Header("Sons (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip   soundHoverEnter;
    [SerializeField] private AudioClip   soundHoverExit;
    [SerializeField] private AudioClip   soundClick;

    // ?????????????????????????????????????????????
    //  INTERNALS
    // ?????????????????????????????????????????????
    private RectTransform rt;
    private float         baseFontSize;
    private Color         baseImageColor;
    private Vector3       baseScale;
    private bool          appeared = false;

    // ?????????????????????????????????????????????
    //  LIFECYCLE
    // ?????????????????????????????????????????????
    private void Awake()
    {
        rt = GetComponent<RectTransform>();

        if (targetImage == null)
            targetImage = transform.Find("Image")?.GetComponent<Image>()
                          ?? GetComponent<Image>();

        if (targetText == null)
            targetText = GetComponentInChildren<TextMeshProUGUI>();

        if (targetText != null)
        {
            baseFontSize = targetText.fontSize;
            targetText.color = textColorNormal;
        }

        if (targetImage != null)
            baseImageColor = targetImage.color;

        baseScale = rt.localScale;
    }

    private void OnEnable()
    {
        // Anima entrada toda vez que o bot�o for ativado
        PlayAppear();
    }

    // ?????????????????????????????????????????????
    //  P�BLICOS � conecte nos EventTrigger / Button
    // ?????????????????????????????????????????????
    public void OnHoverEnter()
    {
        LeanTween.cancel(rt);
        AudioManager.PlaySound(1);

        switch (preset)
        {
            case ButtonPreset.Gentle:
                LeanTween.scale(rt, baseScale * 1.06f, 0.35f).setEase(LeanTweenType.easeOutSine);
                AnimateTextColor(textColorHover, 0.3f);
                break;

            case ButtonPreset.Warm:
                LeanTween.scale(rt, baseScale * 1.08f, 0.3f).setEase(LeanTweenType.easeOutSine);
                LeanTween.rotateZ(rt.gameObject, 1.5f, 0.3f).setEase(LeanTweenType.easeOutSine);
                AnimateTextColor(textColorHover, 0.3f);
                AnimateTextSize(baseFontSize * 1.07f, 0.3f);
                AnimateImageColor(Color.white, 0.3f);
                break;

            case ButtonPreset.Whisper:
                AnimateImageAlpha(1f, 0.4f);
                AnimateTextAlpha(1f, 0.4f);
                break;

            case ButtonPreset.Ink:
                LeanTween.scale(rt, baseScale * 1.04f, 0.4f).setEase(LeanTweenType.easeOutSine);
                AnimateTextColor(textColorHover, 0.35f);
                break;
        }
    }

    public void OnHoverExit()
    {
        LeanTween.cancel(rt);
        AudioManager.PlaySound(2);

        switch (preset)
        {
            case ButtonPreset.Gentle:
                LeanTween.scale(rt, baseScale, 0.4f).setEase(LeanTweenType.easeOutSine);
                AnimateTextColor(textColorNormal, 0.4f);
                break;

            case ButtonPreset.Warm:
                LeanTween.scale(rt, baseScale, 0.4f).setEase(LeanTweenType.easeOutSine);
                LeanTween.rotateZ(rt.gameObject, 0f, 0.35f).setEase(LeanTweenType.easeOutSine);
                AnimateTextColor(textColorNormal, 0.35f);
                AnimateTextSize(baseFontSize, 0.35f);
                AnimateImageColor(baseImageColor, 0.35f);
                break;

            case ButtonPreset.Whisper:
                AnimateImageAlpha(0.55f, 0.5f);
                AnimateTextAlpha(0.6f, 0.5f);
                break;

            case ButtonPreset.Ink:
                LeanTween.scale(rt, baseScale, 0.4f).setEase(LeanTweenType.easeOutSine);
                AnimateTextColor(textColorNormal, 0.4f);
                break;
        }
    }

    public void OnClick()
    {
        LeanTween.cancel(rt);
        AudioManager.PlaySound(0);

        // Squish suave � comprime levemente e volta, sem bounce agressivo
        LeanTween.scale(rt, baseScale * 0.92f, 0.08f)
            .setEase(LeanTweenType.easeInSine)
            .setOnComplete(() =>
                LeanTween.scale(rt, baseScale, 0.3f).setEase(LeanTweenType.easeOutSine));

        // Flash no texto
        AnimateTextColor(textColorClick, 0.08f, () =>
            AnimateTextColor(textColorHover, 0.3f));

        if (preset == ButtonPreset.Warm)
        {
            LeanTween.rotateZ(rt.gameObject, 0f, 0.3f).setEase(LeanTweenType.easeOutSine);
            AnimateImageColor(new Color(1f, 0.95f, 0.8f, 1f), 0.08f, () =>
                AnimateImageColor(baseImageColor, 0.3f));
        }
    }

    // ?????????????????????????????????????????????
    //  APARECER
    // ?????????????????????????????????????????????

    /// <summary>Chame manualmente para reanimar a entrada do bot�o.</summary>
    public void PlayAppear()
    {
        LeanTween.cancel(rt);

        switch (preset)
        {
            case ButtonPreset.Gentle:
            case ButtonPreset.Warm:
                // Sobe suavemente do zero de alpha e de uma escala menor
                rt.localScale = baseScale * 0.88f;
                SetAlpha(0f);
                LeanTween.scale(rt, baseScale, 0.55f).setEase(LeanTweenType.easeOutSine);
                LeanTween.value(rt.gameObject, SetAlpha, 0f, 1f, 0.5f);
                break;

            case ButtonPreset.Whisper:
                // Aparece s� com fade, escala neutra
                SetAlpha(0f);
                LeanTween.value(rt.gameObject, SetAlpha, 0f, 0.55f, 0.6f);
                break;

            case ButtonPreset.Ink:
                // Escala horizontal de 0 ? normal, como texto sendo "escrito"
                rt.localScale = new Vector3(0f, baseScale.y, baseScale.z);
                SetAlpha(0f);
                LeanTween.scaleX(rt.gameObject, baseScale.x, 0.45f).setEase(LeanTweenType.easeOutCubic);
                LeanTween.value(rt.gameObject, SetAlpha, 0f, 1f, 0.4f);
                break;
        }
    }

    // ?????????????????????????????????????????????
    //  HELPERS PRIVADOS
    // ?????????????????????????????????????????????
    private void AnimateTextColor(Color to, float duration, System.Action onComplete = null)
    {
        if (targetText == null) return;
        var from = targetText.color;
        var tween = LeanTween.value(rt.gameObject, c => targetText.color = c, from, to, duration);
        if (onComplete != null) tween.setOnComplete(onComplete);
    }

    private void AnimateTextSize(float to, float duration)
    {
        if (targetText == null) return;
        LeanTween.value(rt.gameObject, v => targetText.fontSize = v, targetText.fontSize, to, duration)
            .setEase(LeanTweenType.easeOutSine);
    }

    private void AnimateTextAlpha(float to, float duration)
    {
        if (targetText == null) return;
        var c = targetText.color;
        LeanTween.value(rt.gameObject, a => { c.a = a; targetText.color = c; }, c.a, to, duration);
    }

    private void AnimateImageColor(Color to, float duration, System.Action onComplete = null)
    {
        if (targetImage == null) return;
        var from = targetImage.color;
        var tween = LeanTween.value(rt.gameObject, c => targetImage.color = c, from, to, duration);
        if (onComplete != null) tween.setOnComplete(onComplete);
    }

    private void AnimateImageAlpha(float to, float duration)
    {
        if (targetImage == null) return;
        var c = targetImage.color;
        LeanTween.value(rt.gameObject, a => { c.a = a; targetImage.color = c; }, c.a, to, duration);
    }

    private void SetAlpha(float a)
    {
        if (targetImage != null)
        {
            var c = targetImage.color;
            c.a = a;
            targetImage.color = c;
        }

        if (targetText != null)
        {
            var c = targetText.color;
            c.a = a;
            targetText.color = c;
        }
    }

    /*private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }*/
}
