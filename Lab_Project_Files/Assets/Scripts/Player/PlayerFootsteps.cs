
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFootsteps : MonoBehaviour
{
    public InputActionReference moveAction;
    public CharacterController characterController;
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.45f;
    public PlayerMovement playerscript;

    float stepTimer;
    int lastSound = -1;

    void Update()
    {
        if (!playerscript.playerCamera.gameObject.activeSelf || playerscript.gameManager.LockPlayerMovement || !playerscript.CanMoveStart) return;

        bool moving = moveAction.action.ReadValue<Vector2>().magnitude > 0.1f;

        if (moving && characterController.isGrounded && footstepSounds.Length > 0)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                int soundIndex = Random.Range(0, footstepSounds.Length);

                if (footstepSounds.Length > 1)
                    while (soundIndex == lastSound)
                        soundIndex = Random.Range(0, footstepSounds.Length);

                lastSound = soundIndex;
                audioSource.PlayOneShot(footstepSounds[soundIndex]);
                stepTimer = stepInterval;
            }
        }
        else stepTimer = 0f;
    }
}
