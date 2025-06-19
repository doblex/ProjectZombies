using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;

    [Header("Camera Settings")]
    [SerializeField] Transform cameraLock;
    [SerializeField] CinemachineCamera cinemachineCamera;
    private float previousFOV = 60f;
    [SerializeField] float sensitivity = 100f;
    [SerializeField] float maxDownCamera;
    [SerializeField] float maxUpCamera;

    [Header("Crouch Settings")]
    [SerializeField] float crouchScale;
    [SerializeField] float crouchSpeed;

    [Header("Climb Settings")]
    [SerializeField] LayerMask climbableLayer;
    [SerializeField] float maxDistanceFromWall;
    [SerializeField] Vector3 rayOriginOffset;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    [SerializeField] float maxHeight;
    [SerializeField] float climbSpeed;

    [Header("Debug")]
    [SerializeField] bool activateDebug;

    Rigidbody rb;
    PlayerInput playerInput;
    float rotationX = 0f;
    float rotationY = 0f;
    bool crouching = false;
    Vector3 climbTarget;
    bool isClimbing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        previousFOV = cinemachineCamera.Lens.FieldOfView;
    }

    private void FixedUpdate()
    {
        if (isClimbing) { return; }
        Move(playerInput.MoveInput.x, playerInput.MoveInput.y);
    }

    void Update()
    {
        if (activateDebug) { CheckForLedge(); }
        if (isClimbing)
        {
            Vector3 direction = (climbTarget - transform.position);
            rb.MovePosition(transform.position + direction.normalized * climbSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, climbTarget) < 0.1f)
            {
                isClimbing = false;
                rb.useGravity = true;
            }
            return;
        }
        Rotate();
        Crouch();
        Climb();
    }

    // OLD
    private void Move(float inputX, float inputZ)
    {
        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;

        float actualSpeed = crouching ? crouchSpeed : Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        // Cambia FOV
        float fov = actualSpeed == sprintSpeed ? 80f : 60f;
        previousFOV = cinemachineCamera.Lens.FieldOfView;
        ChangeFOV(fov);

        // Trasforma la direzione in local space nella direzione del transform
        Vector3 moveDirection = transform.TransformDirection(inputDirection) * actualSpeed;

        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    private void ChangeFOV(float fov)
    {
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(previousFOV, fov, Time.deltaTime * 5f);
    }

    private void Rotate()
    {
        float mouseX = playerInput.LookInput.x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.LookInput.y * sensitivity * Time.deltaTime;

        // Rotazione verticale (guardare su/giù)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxUpCamera, maxDownCamera); // Limita l'inclinazione

        // Rotazione orizzontale (guardare a destra/sinistra)
        rotationY += mouseX;

        transform.localRotation = Quaternion.Euler(0, rotationY, 0f);
        cameraLock.localRotation = Quaternion.Euler(rotationX, 0, 0f);
    }

    private void Crouch()
    {
        if (playerInput.IsCrouching)
        {
            crouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            return;
        }
        else
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
    }

    private void Climb()
    {
        if (playerInput.IsClimbing)
        {
            if (CheckForLedge())
            {
                isClimbing = true;
                rb.useGravity = false;
            }
        }
    }

    private bool CheckForLedge()
    {
        Debug.DrawRay(transform.position + rayOriginOffset, transform.forward * maxDistanceFromWall, Color.red);
        if (Physics.Raycast(transform.position + rayOriginOffset, transform.forward, out RaycastHit wallHit, maxDistanceFromWall, climbableLayer))
        {
            float angle = Vector3.Angle(Vector3.up, wallHit.normal);
            if (minAngle < angle && angle < maxAngle)
            {
                Vector3 topRayOrigin = wallHit.point + Vector3.up * maxHeight + transform.forward * 0.2f;
                Debug.DrawRay(topRayOrigin, Vector3.down * maxHeight, Color.red);
                if (Physics.Raycast(topRayOrigin, Vector3.down, out RaycastHit ledgeHit, maxHeight, climbableLayer))
                {
                    climbTarget = ledgeHit.point;
                    return true;
                }
            }
        }
        return false;
    }
}
