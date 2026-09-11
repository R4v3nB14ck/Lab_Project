using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Acertijo_1 : MonoBehaviour
{
    public bool CanOpenDoor = false;
    public Transform LeftDoor;
    public Transform RightDoor;

    public GraphicRaycaster PanelCanvas;
    public TMP_Dropdown Enunciado1;
    public TMP_Dropdown Enunciado2;
    public TMP_Dropdown Enunciado3;

    public CinemachineCamera PanelFrameCam;
    public CinemachineCamera DoorsFrameCam;

    public BoxCollider InteractionCollider;

    public GameManager gameManager;

    public PlayerMovement PlayerScript;

    public void InteractFocus()
    {
        PanelCanvas.enabled = true;
        PanelFrameCam.enabled = true;
        gameManager.EnableInteractionMode();
        gameManager.PlayerCam.SetActive(false);
        gameManager.UnlockCursor();
    }

    public void CheckCombination()
    {
        if(Enunciado1.value == 3 && Enunciado2.value == 1 && Enunciado3.value == 2 && !CanOpenDoor)
        {
            StartCoroutine(UnlockDoors());
        }
    }

    IEnumerator UnlockDoors()
    {
        CanOpenDoor = true;
        InteractionCollider.enabled = false;
        PanelCanvas.enabled = false;
        DoorsFrameCam.enabled = true;
        PanelFrameCam.enabled = false;
        PlayerScript.Teleport(new Vector3(7.92500019f, 0, 7.5f), new Quaternion(0, -0.707106769f, 0, 0.707106829f));

        yield return new WaitForSeconds(3f);

        gameManager.PlayerCam.SetActive(true);
        DoorsFrameCam.enabled = false;

        yield return new WaitForSeconds(1f);

        gameManager.DisableInteractionMode();
        gameManager.LockCursor();
    }

    IEnumerator CancelInteraction()
    {
        PanelCanvas.enabled = false;
        gameManager.PlayerCam.SetActive(true);
        PanelFrameCam.enabled = false;

        Enunciado1.Hide();
        Enunciado2.Hide();
        Enunciado3.Hide();

        yield return new WaitForSeconds(1f);

        gameManager.DisableInteractionMode();
        gameManager.LockCursor();
    }

    private void Update()
    {
        if(gameManager.CancelAction.action.WasPressedThisFrame() && PanelCanvas.enabled)
        {
            StartCoroutine(CancelInteraction());
        }

        if(CanOpenDoor)
        {
            LeftDoor.rotation = Quaternion.Lerp(LeftDoor.rotation, new Quaternion(-0.430459499f, -0.560985506f, -0.560985386f, 0.43045944f), Time.deltaTime * 2);
            RightDoor.rotation = Quaternion.Lerp(RightDoor.rotation, new Quaternion(-0.430459499f, 0.560985506f, 0.560985386f, 0.43045944f), Time.deltaTime * 2);
        }
        else
        {
            LeftDoor.eulerAngles = new Vector3(270, 0, 0);
            RightDoor.eulerAngles = new Vector3(270, 0, 0);
        }
    }
}
