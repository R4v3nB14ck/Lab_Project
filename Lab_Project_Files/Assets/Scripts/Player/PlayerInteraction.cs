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
        if (gameManager.LockPlayerMovement || !playerCamera.gameObject.activeSelf || !gameManager.Player.gameObject.activeSelf)
        {
            indicator.SetActive(false);
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            indicator.SetActive(true);

            if (InteractAction.action.WasPressedThisFrame())
            {
                if(hit.transform.tag == "Interactive")
                {
                    hit.transform.GetComponent<InteractionEvent>().Interact();
                }
            }
        }
        else
        {
            indicator.SetActive(false);
        }
    }

}