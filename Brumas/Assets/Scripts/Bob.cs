using UnityEngine;

/// <summary>
/// Simula o movimento natural de uma pessoa parada conversando.
/// Adicione diretamente na câmera.
/// </summary>
public class CameraBob : MonoBehaviour
{
    [Header("Respiração")]
    [SerializeField] private float breathSpeed = 1.2f;
    [SerializeField] private float breathAmountY = 0.008f; // sobe/desce
    [SerializeField] private float breathRotationX = 0.15f;  // leve inclinação frente/trás

    [Header("Balanço Lateral")]
    [SerializeField] private float swaySpeed = 0.65f;
    [SerializeField] private float swayAmountX = 0.003f; // deriva lateral
    [SerializeField] private float swayRotZ = 0.3f;   // inclinação em Z (graus)

    [Header("Virada de Cabeça")]
    [SerializeField] private float headTurnMinInterval = 3f;
    [SerializeField] private float headTurnMaxInterval = 8f;
    [SerializeField] private float headTurnMaxAngle = 4f;   // graus
    [SerializeField] private float headTurnDuration = 1.1f;

    // ──────────────────────────────────────────────
    private Vector3 originPos;
    private Quaternion originRot;
    private float timer;
    private float nextTurnTime;
    private float currentTurnY; // offset atual da virada

    private void Awake()
    {
        originPos = transform.localPosition;
        originRot = transform.localRotation;
    }

    private void Start()
    {
        // Pequeno offset no timer para não começar no ponto zero da senoide
        timer = Random.Range(0f, Mathf.PI * 2f);
        ScheduleNextTurn();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        ApplyBob();
        CheckHeadTurn();
    }

    // ──────────────────────────────────────────────
    //  BOB CONTÍNUO
    // ──────────────────────────────────────────────
    private void ApplyBob()
    {
        // Respiração — sobe/desce + leve inclinação no eixo X
        float bobY = Mathf.Sin(timer * breathSpeed) * breathAmountY;
        float rotX = Mathf.Sin(timer * breathSpeed) * breathRotationX;

        // Balanço lateral — deriva em X + leve rolagem em Z
        float bobX = Mathf.Sin(timer * swaySpeed * 0.5f) * swayAmountX;
        float rotZ = Mathf.Sin(timer * swaySpeed) * swayRotZ;

        transform.localPosition = originPos + new Vector3(bobX, bobY, 0f);

        // Combina bob + virada de cabeça atual
        Quaternion bobRotation = Quaternion.Euler(rotX, currentTurnY, rotZ);
        transform.localRotation = originRot * bobRotation;
    }

    // ──────────────────────────────────────────────
    //  VIRADA DE CABEÇA OCASIONAL
    // ──────────────────────────────────────────────
    private void CheckHeadTurn()
    {
        if (Time.time < nextTurnTime) return;

        // Escolhe: virar levemente para um lado, outro, ou voltar ao centro
        float[] options = { -headTurnMaxAngle, -headTurnMaxAngle * 0.4f, 0f, headTurnMaxAngle * 0.4f, headTurnMaxAngle };
        float target = options[Random.Range(0, options.Length)];

        LeanTween.value(gameObject, v => currentTurnY = v, currentTurnY, target, headTurnDuration)
            .setEase(LeanTweenType.easeInOutSine);

        ScheduleNextTurn();
    }

    private void ScheduleNextTurn()
    {
        nextTurnTime = Time.time + Random.Range(headTurnMinInterval, headTurnMaxInterval);
    }
}