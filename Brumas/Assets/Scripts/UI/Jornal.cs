using System.Collections;
using UnityEngine;

public class Jornal : MonoBehaviour
{
    [Header("Tempos")]
    public float intervaloEntreJornais = 0.5f;
    public float duracaoEntrada = 0.4f;
    public float duracaoFade = 1f;

    [Header("Delay")]
    public float atrasoInicial = 0f;

    [Header("Movimento")]
    public float alturaMin = 0.2f;
    public float alturaMax = 0.5f;

    [Header("Escala")]
    public float escalaInicial = 0.8f;

    [Header("Rotação")]
    public float rotacaoAleatoria = 10f;

    private SpriteRenderer[] jornais;
    private Vector3[] posicoesOriginais;
    private Quaternion[] rotacoesOriginais;
    private Vector3[] escalasOriginais;

    void Start()
    {
        int total = transform.childCount;

        jornais = new SpriteRenderer[total];
        posicoesOriginais = new Vector3[total];
        rotacoesOriginais = new Quaternion[total];
        escalasOriginais = new Vector3[total];

        for (int i = 0; i < total; i++)
        {
            Transform filho = transform.GetChild(i);

            jornais[i] = filho.GetComponent<SpriteRenderer>();

            if (jornais[i] == null)
            {
                Debug.LogWarning($"{filho.name} não possui SpriteRenderer.");
                continue;
            }

            posicoesOriginais[i] = filho.localPosition;
            rotacoesOriginais[i] = filho.localRotation;
            escalasOriginais[i] = filho.localScale;

            ResetarJornal(i);
        }

        StartCoroutine(MostrarJornais());
    }

    void ResetarJornal(int index)
    {
        if (jornais[index] == null)
            return;

        StopCoroutine(nameof(FadeOut));

        Transform t = jornais[index].transform;

        t.localPosition = posicoesOriginais[index];
        t.localRotation = rotacoesOriginais[index];
        t.localScale = escalasOriginais[index];

        Color c = jornais[index].color;
        c.a = 0;
        jornais[index].color = c;

        jornais[index].gameObject.SetActive(false);
    }

    IEnumerator MostrarJornais()
    {
        if (atrasoInicial > 0)
            yield return new WaitForSeconds(atrasoInicial);

        while (true)
        {
            // Reseta todos antes de iniciar um novo ciclo
            for (int i = 0; i < jornais.Length; i++)
                ResetarJornal(i);

            for (int i = 0; i < jornais.Length; i++)
            {
                if (jornais[i] == null)
                    continue;

                Transform t = jornais[i].transform;

                t.gameObject.SetActive(true);

                t.localPosition = posicoesOriginais[i] +
                                  Vector3.up * Random.Range(alturaMin, alturaMax);

                t.localScale = escalasOriginais[i] * escalaInicial;

                t.localRotation =
                    rotacoesOriginais[i] *
                    Quaternion.Euler(0, 0, Random.Range(-rotacaoAleatoria, rotacaoAleatoria));

                StartCoroutine(AnimarEntrada(i));

                // Faz todos os anteriores desaparecerem
                for (int j = 0; j < i; j++)
                {
                    if (jornais[j] != null && jornais[j].gameObject.activeSelf)
                        StartCoroutine(FadeOut(jornais[j]));
                }

                yield return new WaitForSeconds(intervaloEntreJornais);
            }

            // Deixa o último visível por um instante
            yield return new WaitForSeconds(intervaloEntreJornais);

            // Some o último antes de reiniciar
            if (jornais.Length > 0 && jornais[^1] != null)
                yield return StartCoroutine(FadeOut(jornais[^1]));

            // Pequena pausa antes de reiniciar o ciclo (opcional)
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator AnimarEntrada(int index)
    {
        SpriteRenderer sr = jornais[index];
        Transform t = sr.transform;

        Vector3 posInicial = t.localPosition;
        Vector3 escalaInicialObj = escalasOriginais[index] * escalaInicial;

        float tempo = 0;

        while (tempo < duracaoEntrada)
        {
            tempo += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, tempo / duracaoEntrada);

            t.localPosition = Vector3.Lerp(posInicial, posicoesOriginais[index], p);
            t.localScale = Vector3.Lerp(escalaInicialObj, escalasOriginais[index], p);

            Color c = sr.color;
            c.a = p;
            sr.color = c;

            yield return null;
        }

        t.localPosition = posicoesOriginais[index];
        t.localScale = escalasOriginais[index];

        Color cor = sr.color;
        cor.a = 1;
        sr.color = cor;
    }

    IEnumerator FadeOut(SpriteRenderer sr)
    {
        float alphaInicial = sr.color.a;
        float tempo = 0;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;

            Color c = sr.color;
            c.a = Mathf.Lerp(alphaInicial, 0, tempo / duracaoFade);
            sr.color = c;

            yield return null;
        }

        Color final = sr.color;
        final.a = 0;
        sr.color = final;

        sr.gameObject.SetActive(false);
    }
}