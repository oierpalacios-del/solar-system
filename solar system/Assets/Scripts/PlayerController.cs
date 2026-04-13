using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float rotationSpeed = 10f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 gravityDir = Vector3.down;
    private float gravityStrength = 20f;
    public bool isGrounded;
    private PlanetGravity currentPlanet;

    public bool IsJumping { get; private set; }

    // Al inicio, cachea todos los planetas
    private PlanetGravity[] _allPlanets;

    private Vector3 _lastPlanetPosition;
    private PlanetGravity _currentPlanet;

    void FixedUpdate()
    {
        FindNearestPlanetGravity();
        ApplyPlanetMovement(); // Añadir antes del resto
        ApplyGravity();
        HandleMovement();
        AlignToGravity();
    }

    private void ApplyPlanetMovement()
    {
        if (_currentPlanet == null) return;

        Vector3 currentPlanetPos = _currentPlanet.transform.position;
        Vector3 planetDelta = currentPlanetPos - _lastPlanetPosition;

        // Solo arrastrar al jugador si está en superficie o muy cerca
        float distToPlanet = Vector3.Distance(transform.position, currentPlanetPos);
        if (distToPlanet < _currentPlanet.gravityRadius * 0.6f)
        {
            transform.position += planetDelta;
        }

        _lastPlanetPosition = currentPlanetPos;
    }

    private void FindNearestPlanetGravity()
    {
        if (_allPlanets == null || _allPlanets.Length == 0) return;

        PlanetGravity nearest = null;
        float nearestDist = float.MaxValue;

        foreach (PlanetGravity planet in _allPlanets)
        {
            if (planet == null) continue;
            float dist = Vector3.Distance(transform.position, planet.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = planet;
            }
        }

        if (nearest != null)
        {
            // Actualizar referencia y cachear posición si cambia de planeta
            if (nearest != _currentPlanet)
            {
                _currentPlanet = nearest;
                _lastPlanetPosition = nearest.transform.position;
            }

            Vector3 dirToCenter = (nearest.transform.position - transform.position).normalized;
            float adjustedGravity = nearest.gravity *
                                    Mathf.Clamp01(nearest.gravityRadius /
                                    Vector3.Distance(transform.position, nearest.transform.position));
            SetGravity(dirToCenter, adjustedGravity, nearest);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        _allPlanets = FindObjectsByType<PlanetGravity>(FindObjectsSortMode.None);
    }

    void Update()
    {
        CheckGrounded();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void ApplyGravity()
    {
        rb.AddForce(gravityDir * gravityStrength, ForceMode.Acceleration);
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
            return;

        // Movimiento relativo a la orientación del jugador
        Vector3 move = (transform.right * h + transform.forward * v).normalized * moveSpeed;

        // Conservar velocidad en dirección de la gravedad
        Vector3 gravityVelocity = Vector3.Project(rb.linearVelocity, gravityDir);
        rb.linearVelocity = move + gravityVelocity;
    }

    private void AlignToGravity()
    {
        Vector3 targetUp = -gravityDir;
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, gravityDir); // Eliminar velocidad vertical
        rb.AddForce(-gravityDir * jumpForce, ForceMode.Impulse);
        IsJumping = true;
    }

    private void CheckGrounded()
    {
        // Raycast desde un poco arriba del centro hacia "abajo" (dirección de gravedad)
        Vector3 origin = transform.position - gravityDir * 0.1f;
        isGrounded = Physics.Raycast(origin, gravityDir, groundCheckDistance, groundLayer);

        if (isGrounded)
            IsJumping = false;
    }

    public void SetGravity(Vector3 directionToCenter, float strength, PlanetGravity planet)
    {
        gravityDir = directionToCenter;
        gravityStrength = strength;
        currentPlanet = planet;
    }

    // Debug visual
    void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 origin = transform.position - gravityDir * 0.1f;
        Gizmos.DrawLine(origin, origin + gravityDir * groundCheckDistance);
    }
}
