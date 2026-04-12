using UnityEngine;

/// <summary>
/// Rotates a planet around its own axis.
/// Attach this component to any planet GameObject.
/// </summary>
public class PlanetRotation : MonoBehaviour
{
    [Header("Rotation Speed")]
    [Tooltip("Degrees per second. Negative value reverses direction (e.g. Venus).")]
    public float rotationSpeed = 30f;

    [Header("Axis")]
    [Tooltip("Local axis of rotation. (0,1,0) = Y axis (standard upright spin).")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Axial tilt in degrees applied at Start. E.g. Earth = 23.5, Uranus = 97.7")]
    [Range(0f, 180f)]
    public float axialTilt = 0f;

    [Header("Playback")]
    [Tooltip("Multiplier applied on top of rotationSpeed. Useful for global time-scale sliders.")]
    public float timeScale = 1f;
    public bool isPaused = false;

    void Start()
    {
        // Apply axial tilt around the Z axis once at initialisation
        transform.Rotate(Vector3.forward, axialTilt, Space.World);
    }

    void Update()
    {
        if (isPaused) return;

        float delta = rotationSpeed * timeScale * Time.deltaTime;
        transform.Rotate(rotationAxis.normalized, delta, Space.Self);
    }

    /// <summary>Toggle pause from external scripts or UI buttons.</summary>
    public void TogglePause() => isPaused = !isPaused;
}
