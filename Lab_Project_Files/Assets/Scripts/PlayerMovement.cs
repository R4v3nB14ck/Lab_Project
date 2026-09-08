using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5.0f;

    [Header("Mirada")]
    public Transform playerCamera;
    public float mouseSensitivity = 1; // El nuevo input maneja valores más altos, baja esto si es muy sensible
    public float LookLimit = 80.0f;

    [Header("Referencias del Nuevo Input System")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;

    [Header("Otras Referencias")]
    public CharacterController characterController;
    public GameManager gameManager;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!playerCamera.gameObject.activeSelf || gameManager.OnInteractionCam) return;

        // --- 1. ROTACIÓN (MIRAR) usando Vector2 ---
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        float sensmultiplier = gameManager ? gameManager.MouseSensMultiplier : 1;

        float mouseX = lookInput.x * (mouseSensitivity * sensmultiplier) * Time.unscaledDeltaTime;
        float mouseY = lookInput.y * (mouseSensitivity * sensmultiplier) * Time.unscaledDeltaTime;

        // Rotación horizontal (Cuerpo)
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical (Cámara)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -LookLimit, LookLimit);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);


        // --- 2. MOVIMIENTO (CAMINAR) usando Vector2 ---
        Vector2 inputDirection = moveAction.action.ReadValue<Vector2>();

        // Convertimos el Vector2 (X, Y de la pantalla/stick) al espacio 3D (X, Z del mundo)
        Vector3 forwardMovement = transform.forward * inputDirection.y;
        Vector3 rightMovement = transform.right * inputDirection.x;

        // Combinamos y aplicamos velocidad física
        float currentY = moveDirection.y; // Conservamos la gravedad actual
        moveDirection = (forwardMovement + rightMovement) * speed;

        // Aplicamos gravedad básica
        if (!characterController.isGrounded)
        {
            currentY += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            currentY = -0.5f;
        }

        moveDirection.y = currentY;

        // Movemos el personaje
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        verticalRotation = 0f;
        moveDirection = Vector3.zero;
        transform.position = position;
        transform.rotation = rotation;
    }
}