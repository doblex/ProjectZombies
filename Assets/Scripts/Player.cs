using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform cameraLock;
    [SerializeField] float sensitivity = 100f;
    private Rigidbody rb;
    float rotationX = 0f;
    float rotationY = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void Update()
    {
        Rotate();
    }

    private void Move(float inputX, float inputZ)
    {
        rb.linearVelocity = new Vector3(inputX, 0, inputZ).normalized * speed + new Vector3(0, rb.linearVelocity.y, 0);
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Rotazione verticale (guardare su/giù)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limita l'inclinazione

        // Rotazione orizzontale (guardare a destra/sinistra)
        rotationY += mouseX;

        transform.localRotation = Quaternion.Euler(0, rotationY, 0f);
        cameraLock.localRotation = Quaternion.Euler(rotationX, 0, 0f);
    }
}
