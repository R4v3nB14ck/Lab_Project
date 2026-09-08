using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float maxDistance = 1.5f;
    public LayerMask interactableLayer;
    public GameObject indicator;
    public Transform playerCamera;
    public InputActionReference InteractAction;
    public GameManager gameManager;

    void Update()
    {
        if (gameManager.OnInteractionCam)
        {
            indicator.SetActive(false);
            return;
        }

        if (!playerCamera.gameObject.activeSelf) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            indicator.SetActive(true);

            if (InteractAction.action.WasPressedThisFrame())
            {
                if(hit.transform.tag == "Acertijo1")
                {
                    hit.transform.GetComponent<Acertijo_1>().InteractFocus();
                }
            }
        }
        else
        {
            indicator.SetActive(false);
        }
    }

}