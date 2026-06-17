using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject MobileControls;
    public InputActionReference PauseAction;
    public InputActionReference CancelAction;
    public GameObject PauseMenu;
    public GameObject MainMenuCamera;
    public GameObject PlayerCam;
    public GameObject HUD;
    public GameObject InteractingHUD;
    public float MouseSensMultiplier = 1;
    public bool OnInteractionCam = false;

    private void Start()
    {
        Pause();
    }

    public void SetMouseSensMultiplier(float value)
    {
        MouseSensMultiplier = value;
    }

    public void Pause()
    {
        UnlockCursor();
        PauseMenu.SetActive(true);
        MainMenuCamera.SetActive(true);
        PlayerCam.SetActive(false);
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
        PlayerCam.SetActive(true);
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

    public void EnableInteractionMode()
    {
        OnInteractionCam = true;
        InteractingHUD.SetActive(true);
    }

    public void DisableInteractionMode()
    {
        OnInteractionCam = false;
        InteractingHUD.SetActive(false);
    }

    void Update()
    {
        //Pause Menu
        if (PauseAction.action.WasPressedThisFrame() && !OnInteractionCam) if (PauseMenu.activeSelf) UnPause(); else Pause();

        MobileControls.SetActive((Application.isMobilePlatform || IsRunningInSimulator()) && !PauseMenu.activeSelf && !OnInteractionCam);
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