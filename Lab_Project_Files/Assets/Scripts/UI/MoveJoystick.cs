using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.Layouts;

public class MoveJoystick : OnScreenControl, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Referencias")]
    public RectTransform background;
    public RectTransform handle;

    [Header("Configuración")]
    public float movementRange = 50f;

    [InputControl(layout = "Vector2")] // Esto hace que aparezca el selector de path
    [SerializeField] private string m_ControlPath;

    private CanvasGroup canvasGroup;
    private Vector2 pointerDownPos;

    protected override string controlPathInternal { get => m_ControlPath; set => m_ControlPath = value; }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;

        // Movemos el conjunto al lugar del toque
        background.position = eventData.position;
        handle.position = eventData.position;
        pointerDownPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - pointerDownPos;

        // Limitamos el radio
        Vector2 clampedDelta = Vector2.ClampMagnitude(delta, movementRange);

        // El fondo se queda fijo, solo movemos el handle
        handle.position = pointerDownPos + clampedDelta;

        // Enviamos el valor normalizado (-1 a 1) al sistema de input
        SendValueToControl(clampedDelta / movementRange);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
        handle.position = pointerDownPos;

        // Enviamos valor cero al soltar
        SendValueToControl(Vector2.zero);
    }
}