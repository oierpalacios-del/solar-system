using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        Vector3 dir = (transform.position - other.transform.position).normalized;
        pc.SetGravityDirection(-dir);
        pc.SetCurrentPlanet(GetComponent<PlanetGravity>());
    }
}