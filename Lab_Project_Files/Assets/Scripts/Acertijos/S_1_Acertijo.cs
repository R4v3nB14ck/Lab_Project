using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_1_Acertijo : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera KeyPadCam;
    public CinemachineCamera DoorCam;

    [Header("Input")]
    public InputActionReference backAction;

    [Header("Code")]
    public TextMeshProUGUI CodeText;
    public string CorrectCode = "85163427";
    bool canCheckCode = true;

    [Header("Sound")]
    public AudioSource KeyPadAS;
    public AudioClip CorrectSound;
    public AudioClip WrongSound;

    [Header("UI")]
    public GameObject GreenLight;
    public GameObject RedLight;
    public GameObject CodePreview;
    public GraphicRaycaster keypadRaycaster;

    [Header("Dialogue")]
    public string[] firstDialogueLines;
    bool firstDialoguestarted = false;

    public string[] errorDialogueLines;
    public string errorSingleLine;
    bool alreadysawerror = false;

    [Header("References")]
    public GameManager gameManager;
    public LabDoors doorScript;

    private void Start()
    {
        /*
        if(doorScript.DoorOpen)
        {
            doorScript.DoorOpen = false;
            gameManager.UnPause();
            gameManager.LockPlayerMovement = false;
        }*/

        ClearCode();

        CodePreview.SetActive(false);
    }

    private void Update()
    {
        if(backAction.action.WasPressedThisFrame())
        {
            Invoke("UnFocusKeypad", 0.1f);
        }

        if (firstDialoguestarted && !gameManager.dialogueScript.DialoguePage.activeSelf)
        {
            firstDialoguestarted = false;
            gameManager.LockPlayerMovement = false;
            gameManager.LockCursor();
            gameObject.SetActive(false);
        }
    }

    public void FocusKeypad()
    {
        gameManager.LockPlayerMovement = true;
        gameManager.InteractingHUD.SetActive(true);
        KeyPadCam.enabled = true;
        gameManager.Player.gameObject.SetActive(false);
        gameManager.UnlockCursor();
        CodePreview.SetActive(true);
        keypadRaycaster.enabled = true;
    }

    public void UnFocusKeypad()
    {
        if (gameManager.dialogueScript.DialoguePage.activeSelf || !canCheckCode || !KeyPadCam.enabled) return;

        gameManager.InteractingHUD.SetActive(false);
        gameManager.Player.gameObject.SetActive(true);
        KeyPadCam.enabled = false;
        gameManager.LockPlayerMovement = false;
        gameManager.LockCursor();
        CodePreview.SetActive(false);
        keypadRaycaster.enabled = false;
    }

    public void TypeNumber(string number)
    {
        if(CodeText.text.Length < 8)
        {
            CodeText.text += number;
        }
    }

    public void ClearCode()
    {
        CodeText.text = "";
    }

    public void CheckCode()
    {
        if (!canCheckCode) return;

        if(CodeText.text == CorrectCode)
        {
            GreenLight.SetActive(true);
            KeyPadAS.PlayOneShot(CorrectSound);
            StartCoroutine(OpenDoor());
        }
        else
        {
            canCheckCode = false;
            keypadRaycaster.enabled = false;
            gameManager.InteractingHUD.SetActive(false);
            RedLight.SetActive(true);
            KeyPadAS.PlayOneShot(WrongSound);
            StartCoroutine(WrongCodeDialogue());
        }

        ClearCode();

        Invoke("TurnOffLights", 1);
    }

    IEnumerator WrongCodeDialogue()
    {
        yield return new WaitForSeconds(1);

        if(alreadysawerror)
        {
            string[] singlelineerror = { errorSingleLine };
            gameManager.dialogueScript.startDialogue(singlelineerror);
        }
        else
        {
            gameManager.dialogueScript.startDialogue(errorDialogueLines);
            alreadysawerror = true;
        }

        yield return new WaitForSeconds(1);

        gameManager.InteractingHUD.SetActive(true);
        keypadRaycaster.enabled = true;
        canCheckCode = true;
    }

    void TurnOffLights()
    {
        GreenLight.SetActive(false);
        RedLight.SetActive(false);
    }

    IEnumerator OpenDoor()
    {
        gameManager.LockPlayerMovement = true;

        keypadRaycaster.enabled = false;
        DoorCam.enabled = true;
        KeyPadCam.enabled = false;
        gameManager.InteractingHUD.SetActive(false);
        gameManager.Player.playerCamera.gameObject.SetActive(false);
        gameManager.Player.Teleport(new Vector3(8, 0, 7.5f), new Quaternion(0, -0.707106769f, 0, 0.707106829f));
        doorScript.DoorOpen = true;

        yield return new WaitForSeconds(2);

        gameManager.Player.gameObject.SetActive(true);
        gameManager.Player.playerCamera.gameObject.SetActive(true);
        DoorCam.enabled = false;

        KeyPadCam.enabled = false;
        CodePreview.SetActive(false);
        keypadRaycaster.enabled = false;

        yield return new WaitForSeconds(1);

        gameManager.dialogueScript.startDialogue(firstDialogueLines);
        firstDialoguestarted = true;
    }
}
