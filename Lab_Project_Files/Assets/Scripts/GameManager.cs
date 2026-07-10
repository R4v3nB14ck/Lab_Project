using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public GameObject MobileControls;
    bool lastMobileState;

    void Update()
    {
        bool mobile = Application.isMobilePlatform || Touchscreen.current != null;

        if (mobile != lastMobileState)
        {
            lastMobileState = mobile;
            MobileControls.SetActive(mobile);
        }
    }
}
