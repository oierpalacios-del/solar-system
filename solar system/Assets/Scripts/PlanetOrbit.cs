using UnityEngine;

/// <summary>
/// Moves a planet in an elliptical orbit around a central body (e.g. the Sun).
/// Attach this component to any planet GameObject.
/// </summary>
public class PlanetOrbit : MonoBehaviour
{
    [Header("Central Body")]
    [Tooltip("The GameObject to orbit around (assign the Sun here).")]
    public Transform centralBody;

    [Header("Ellipse Shape")]
    [Tooltip("Semi-major axis: max distance from the central body (X direction).")]
    public float semiMajorAxis = 10f;

    [Tooltip("Semi-minor axis: min distance from the central body (Z direction). " +
             "Set equal to semiMajorAxis for a circular orbit.")]
    public float semiMinorAxis = 8f;

    [Header("Orbit Speed")]
    [Tooltip("Degrees per second around the orbit.")]
    public float orbitSpeed = 20f;

    [Header("Orbit Plane")]
    [Tooltip("Tilt of the orbital plane in degrees (0 = flat on XZ plane).")]
    [Range(0f, 90f)]
    public float orbitalInclination = 0f;

    [Tooltip("Rotation of the ellipse around the Y axis (argument of periapsis).")]
    [Range(0f, 360f)]
    public float periapsisAngle = 0f;

    [Header("Playback")]
    [Tooltip("Multiplier applied on top of orbitSpeed. Use a global manager to sync all planets.")]
    public float timeScale = 1f;
    public bool isPaused = false;

    [Header("Debug")]
    [Tooltip("Draw the orbit path in the Scene view.")]
    public bool showOrbitGizmo = true;
    public Color gizmoColor = new Color(0.3f, 0.7f, 1f, 0.5f);

    // Current angle along the orbit in degrees
    private float _currentAngle = 0f;
    private Quaternion _orbitPlaneRotation;

    void Start()
    {
        // Randomise starting position so planets don't all begin at the same point
        _currentAngle = Random.Range(0f, 360f);

        // Pre-compute the combined rotation for inclination + periapsis offset
        _orbitPlaneRotation = Quaternion.Euler(orbitalInclination, periapsisAngle, 0f);

        if (centralBody == null)
            Debug.LogWarning($"[PlanetOrbit] '{name}': centralBody is not assigned!", this);
    }

    void Update()
    {
        if (isPaused || centralBody == null) return;

        _currentAngle += orbitSpeed * timeScale * Time.deltaTime;
        if (_currentAngle >= 360f) _currentAngle -= 360f;

        transform.position = CalculatePosition(_currentAngle);
    }

    /// <summary>
    /// Returns the world-space position on the ellipse for a given angle (degrees).
    /// </summary>
    private Vector3 CalculatePosition(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;

        // Local ellipse point (flat on XZ plane)
        Vector3 localPoint = new Vector3(
            semiMajorAxis * Mathf.Cos(rad),
            0f,
            semiMinorAxis * Mathf.Sin(rad)
        );

        // Apply inclination + periapsis rotation, then offset from central body
        return centralBody.position + _orbitPlaneRotation * localPoint;
    }

    /// <summary>Toggle pause from external scripts or UI buttons.</summary>
    public void TogglePause() => isPaused = !isPaused;

    // ─── Scene-view gizmo ────────────────────────────────────────────────────
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!showOrbitGizmo || centralBody == null) return;
        DrawOrbitGizmo();
    }

    private void OnValidate()
    {
        // Recompute rotation when values change in the Inspector
        _orbitPlaneRotation = Quaternion.Euler(orbitalInclination, periapsisAngle, 0f);
    }

    private void DrawOrbitGizmo()
    {
        Gizmos.color = gizmoColor;
        int segments = 64;
        Vector3 prev = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 360f / segments;
            Vector3 point = CalculatePosition(angle);
            if (i > 0) Gizmos.DrawLine(prev, point);
            prev = point;
        }

        // Mark the periapsis point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(CalculatePosition(0f), semiMajorAxis * 0.02f);
    }
#endif
}
