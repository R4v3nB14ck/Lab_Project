using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject MobileControls;
    public InputActionReference PauseAction;
    public GameObject PauseMenu;
    public GameObject MainMenuCamera;
    public float MouseSensMultiplier = 1;

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
        if(Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Cursor.visible = true;
        PauseMenu.SetActive(true);
        MainMenuCamera.SetActive(true);
    }

    public void UnPause()
    {
        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        Cursor.visible = false;
        PauseMenu.SetActive(false);
        MainMenuCamera.SetActive(false);
    }

    void Update()
    {
        //Pause Menu
        if (PauseAction.action.WasPressedThisFrame()) if (PauseMenu.activeSelf) UnPause(); else Pause();

        MobileControls.SetActive((Application.isMobilePlatform || IsRunningInSimulator()) && !PauseMenu.activeSelf);
    }

    public static bool IsRunningInSimulator()
    {
#if UNITY_EDITOR
        // If the mocked device platform differs from the actual Editor platform, 
        // the Device Simulator is active and overriding the system info.
        return UnityEngine.Device.Application.platform != UnityEngine.Application.platform;
#else
        return false;
#endif
    }
}