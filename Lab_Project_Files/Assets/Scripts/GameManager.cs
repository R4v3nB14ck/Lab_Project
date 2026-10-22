using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject MobileControls;
    public GameObject MainMenuCamera;
    public GameObject PauseMenu;
    public GameObject HUD;
    public GameObject InteractingHUD;

    [Header("Input")]
    public InputActionReference PauseAction;
    public InputActionReference CancelAction;

    [Header("Other References")]
    public PlayerMovement Player;
    public DialogueSystem dialogueScript;

    [Header("Config")]
    public float MouseSensMultiplier = 1;
    public bool LockPlayerMovement = true;

    public void SetMouseSensMultiplier(float value)
    {
        MouseSensMultiplier = value;
    }

    public void Pause()
    {
        if (!Player.gameObject.activeSelf) return;

        UnlockCursor();
        PauseMenu.SetActive(true);
        MainMenuCamera.SetActive(true);
        Player.gameObject.SetActive(false);
        HUD.SetActive(false);
    }

    public void UnlockCursor()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Cursor.visible = true;
    }

    public void UnPause()
    {
        LockCursor();
        PauseMenu.SetActive(false);
        MainMenuCamera.SetActive(false);
        Player.gameObject.SetActive(true);
        HUD.SetActive(true);
    }

    public void LockCursor()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = false;
    }

    void Update()
    {
        //Pause Menu
        if (PauseAction.action.WasPressedThisFrame() && !LockPlayerMovement) if (PauseMenu.activeSelf) UnPause(); else Pause();

        MobileControls.SetActive((Application.isMobilePlatform || IsRunningInSimulator()) && !PauseMenu.activeSelf && !LockPlayerMovement);

        HUD.SetActive(!LockPlayerMovement);
    }

    public static bool IsRunningInSimulator()
    {
        #if UNITY_EDITOR
            return UnityEngine.Device.Application.platform != UnityEngine.Application.platform;
        #else
            return false;
        #endif
    }
}