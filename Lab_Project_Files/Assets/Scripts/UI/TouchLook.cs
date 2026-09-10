using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.OnScreen;

// Este script reemplaza al "OnScreenLook" de Unity si quieres control total
public class TouchLook : OnScreenControl, IDragHandler
{
    private string m_ControlPath = "<Mouse>/delta";
    protected override string controlPathInternal { get => m_ControlPath; set => m_ControlPath = value; }

    public void OnDrag(PointerEventData eventData)
    {
        // Enviar el delta del toque al Input System como si fuera el LookAction
        SendValueToControl(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Resetear cuando el dedo se levanta
        SendValueToControl(Vector2.zero);
    }
}