using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example_PlayerMovement : MonoBehaviour
{
    [Header("Action Ref")]
    public InputActionReference moveActionRef;
    public InputActionReference shootActionRef;

    [Header("Player Settings")]
    public float speed = 5f;
    public int maxHealth = 20;
    public event Action<float, float> OnHealthChanged;

    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (_currentHealth != value)
            {
                _currentHealth = value;
                OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            }
        }
    }

    private InputAction moveAction;
    private InputAction shootAction;
    private HudUI hudUI;

    void Start()
    {
        moveAction = moveActionRef?.action;
        if (moveAction != null)
            moveAction.Enable();

        shootAction = shootActionRef?.action;
        if (shootAction != null)
            shootAction.Enable();

        hudUI = HudUI.Instance;

        CurrentHealth = maxHealth;
    }

    void Update()
    {
        if (moveAction != null)
        {
            HandleMovement();
        }

        // shootAction.triggered --> solamente una volta
        // shootAction.IsPressed() --> ogni frame finchè il tasto è premuto
        if (shootAction != null && shootAction.triggered)
        {
            Debug.Log("Shoot action triggered!");
            // Implement shooting logic here
        }

        if (Input.GetKey(KeyCode.Space))
        {
            CurrentHealth--;
        }

        if (Input.GetKey(KeyCode.E))
        {
            CurrentHealth++;
        }
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
