using UnityEngine;

public class planetrotation : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject sun;
    void Start()
    {
        rotationSpeed = 10f;
    }

    void Update()
    {
        if(gameObject.name == "sol")
        {
            transform.rotation = Quaternion.Euler((transform.rotation.x + rotationSpeed) * Time.deltaTime, (transform.rotation.y + rotationSpeed) * Time.deltaTime, (transform.rotation.z + rotationSpeed) * Time.deltaTime);
        }
        else
        {
            float distance = Vector3.Distance(transform.position, sun.transform.position);
            transform.rotation = Quaternion.Euler((transform.rotation.x - distance + rotationSpeed) * Time.deltaTime, (transform.rotation.y - distance + rotationSpeed) * Time.deltaTime, (transform.rotation.z - distance + rotationSpeed) * Time.deltaTime);
        }
    }
}
