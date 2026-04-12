using UnityEngine;

/// <summary>
/// Atrae al jugador hacia la superficie del planeta.
/// Añade este script al planeta junto con un SphereCollider en modo Trigger.
/// </summary>
public class PlanetGravity : MonoBehaviour
{
    [Header("Gravedad")]
    [Tooltip("Fuerza de gravedad hacia el centro del planeta.")]
    public float gravity = 9.81f;

    [Tooltip("Radio del campo gravitacional (debe ser mayor que el planeta).")]
    public float gravityRadius = 10f;

    [Header("Debug")]
    public bool showGizmo = true;

    private SphereCollider gravityField;

    void Start()
    {
        // Crear o configurar el SphereCollider como trigger automáticamente
        gravityField = GetComponent<SphereCollider>();
        if (gravityField == null)
            gravityField = gameObject.AddComponent<SphereCollider>();

        gravityField.isTrigger = true;
        gravityField.radius = gravityRadius;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Dirección desde el jugador hacia el centro del planeta
        Vector3 directionToCenter = (transform.position - other.transform.position).normalized;

        // Aplicar fuerza gravitacional
        rb.AddForce(directionToCenter * gravity, ForceMode.Acceleration);

        // Rotar al jugador para que sus pies apunten al planeta
        AlignPlayerToPlanet(other.transform, directionToCenter);
    }

    private void AlignPlayerToPlanet(Transform player, Vector3 directionToCenter)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(
            -player.up,      // "abajo" del jugador
            directionToCenter // dirección al centro
        ) * player.rotation;

        player.rotation = Quaternion.Slerp(
            player.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Al salir del campo gravitacional, quitar gravedad
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!showGizmo) return;
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.15f);
        Gizmos.DrawSphere(transform.position, gravityRadius);
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }
#endif
}