using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    public float gravityScale = 20f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 gravityDir = Vector3.down;
    private bool isGrounded;
    private PlanetGravity currentPlanet;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void Update()
    {
        CheckGrounded();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    void FixedUpdate()
    {
        rb.AddForce(gravityDir * gravityScale, ForceMode.Acceleration);
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Movimiento relativo a la orientación del jugador
        Vector3 move = transform.right * h + transform.forward * v;
        move = move.normalized * moveSpeed;

        // Conservar velocidad vertical (gravedad)
        Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, gravityDir);
        rb.linearVelocity = move + verticalVelocity;
    }

    private void Jump()
    {
        // Saltar en dirección contraria a la gravedad
        rb.AddForce(-gravityDir * jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            gravityDir,
            groundCheckDistance,
            groundLayer
        );
    }

    public void SetGravityDirection(Vector3 dir)
    {
        gravityDir = dir;

        Quaternion targetRot = Quaternion.FromToRotation(
            transform.up, -dir) * transform.rotation;
        transform.rotation = Quaternion.Slerp(
            transform.rotation, targetRot, 10f * Time.fixedDeltaTime);
    }

    public void SetCurrentPlanet(PlanetGravity planet)
    {
        currentPlanet = planet;
    }
}