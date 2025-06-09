using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region MOVEMENT
    [Header("Input Debugging")]
    [SerializeField] private Vector2 _moveInput;
    public Vector2 MoveInput
    {
        get => _moveInput;
        set
        {
            if (_moveInput != value)
            {
                _moveInput = value;
            }
        }
    }

    void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }
    #endregion

    #region LOOK
    [SerializeField] private Vector2 _lookInput;
    public Vector2 LookInput
    {
        get => _lookInput;
        set
        {
            if (_lookInput != value)
            {
                _lookInput = value;
            }
        }
    }

    void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }
    #endregion

    #region FIRE
    public bool IsFiring;

    void OnFire(InputValue value)
    {
        IsFiring = value.isPressed;
    }
    #endregion

    #region AIM
    public bool IsAiming;
    void OnAim(InputValue value)
    {
        IsAiming = value.isPressed;
    }
    #endregion

    #region REBUILD
    public bool IsRebuilding;
    void OnRebuild(InputValue value)
    {
        IsRebuilding = value.isPressed;
    }
    #endregion

    #region CROUCH
    public bool IsCrouching;
    void OnCrouch(InputValue value)
    {
        IsCrouching = !IsCrouching;
    }
    #endregion

    #region JUMP
    public bool IsJumping;
    void OnJump(InputValue value)
    {
        IsJumping = value.isPressed;
    }
    #endregion

    #region SPRINT
    public bool IsSprinting;
    void OnSprint(InputValue value)
    {
        IsSprinting = value.isPressed;
    }
    #endregion

    #region WEAPONS SELECTING
    public int SelectedWeaponIndex = 0;

    void OnSelectWeapon1(InputValue value)
    {
        SelectedWeaponIndex = 1;
    }
    void OnSelectWeapon2(InputValue value)
    {
        SelectedWeaponIndex = 2;
    }

    void OnScrollWheel()
    {
        if (Mouse.current.scroll.ReadValue().normalized != Vector2.zero)
        {
            SelectedWeaponIndex += (Mouse.current.scroll.ReadValue().y > 0 ? 1 : -1);

            if (SelectedWeaponIndex < 0)
                SelectedWeaponIndex += 2;

            SelectedWeaponIndex %= 2;

            Debug.Log("Index: " + SelectedWeaponIndex);
        }
    }
    #endregion

    #region SWITCH FIRING MODE
    public enum FiringMode
    {
        SingleShot,
        SemiAuto,
        FullAuto
    }
    public FiringMode CurrentFiringMode = FiringMode.SingleShot;

    void OnSwitchFiringMode(InputValue value)
    {
        CurrentFiringMode = (FiringMode)(((int)CurrentFiringMode + 1) % System.Enum.GetValues(typeof(FiringMode)).Length);
    }
    #endregion


    // TESTING
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(MoveInput.x, 0, MoveInput.y) * 5f * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
