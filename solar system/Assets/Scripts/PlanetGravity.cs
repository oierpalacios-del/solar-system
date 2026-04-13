using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [Header("Gravedad")]
    public float gravity = 9.8f;
    public float gravityRadius = 15f;

    [Header("Debug")]
    public bool showGizmo = true;

    void Start()
    {
        // Collider trigger para detectar jugadores en el campo gravitacional
        SphereCollider gravityField = gameObject.AddComponent<SphereCollider>();
        gravityField.isTrigger = true;
        gravityField.radius = gravityRadius;

        // Asegúrate de que el planeta tenga un collider sólido para la superficie
        // (añádelo manualmente en el Editor, NO aquí como trigger)
    }

    // PlanetGravity.cs


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
