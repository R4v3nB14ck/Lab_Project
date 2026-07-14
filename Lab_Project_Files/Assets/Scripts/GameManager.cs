using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject MobileControls;
    public InputActionReference PauseAction;
    public GameObject PauseMenu;


    private void Start()
    {
        Pause();
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        //Pause Menu
        if (PauseAction.action.WasPressedThisFrame()) if (PauseMenu.activeSelf) UnPause(); else Pause();

        MobileControls.SetActive(Application.isMobilePlatform && !PauseMenu.activeSelf);
    }
}