using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el New Input System
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float maxDistance = 1.5f;
    public LayerMask interactableLayer;
    public GameObject indicator; // Tu imagen UI
    public Transform playerCamera;

    private GraphicRaycaster currentCanvasRaycaster;

    void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Comprobamos si miramos al layer "interactable" dentro de la distancia
        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            // Mostramos el indicador
            if (indicator != null) indicator.SetActive(true);

            // Obtenemos el Raycaster del canvas que estamos mirando
            GraphicRaycaster gr = hit.collider.GetComponent<GraphicRaycaster>();

            if (gr != null)
            {
                gr.enabled = true; // Permitimos interactuar
                currentCanvasRaycaster = gr;
            }
        }
        else
        {
            // Si no estamos mirando nada interactuable o estamos lejos:

            // Ocultamos el indicador
            if (indicator != null) indicator.SetActive(false);

            // Deshabilitamos el último raycaster activo para que no se pueda clickear
            if (currentCanvasRaycaster != null)
            {
                currentCanvasRaycaster.enabled = false;
                currentCanvasRaycaster = null;
            }
        }
    }

}