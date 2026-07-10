using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public GameObject MobileControls;
    bool lastMobileState;
    public InputActionReference PauseAction;
    public GameObject PauseMenu;

    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        //Pause Menu
        if (PauseAction.action.WasPressedThisFrame()) if (PauseMenu.activeSelf) Pause(); else UnPause();

        bool mobile = Application.isMobilePlatform;
        if (mobile != lastMobileState)
        {
            lastMobileState = mobile;
            MobileControls.SetActive(mobile);
        }
    }
}