using UnityEngine;

public class interactor : MonoBehaviour
{
    LayerMask mask;
    public float distance = 20f;
    public Texture2D puntero;
    public GameObject TextDetect;
    GameObject ultimoReconozido = null;
    int contadorobj = 0;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("RaycastDetect");
        TextDetect.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, distance, mask))
        {
            Deselect();

            if (hitinfo.collider.tag == "Objeto Interactivo")
            {
                SelectedObject(hitinfo.transform);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    contadorobj++;
                }
            }
        }
        else
        {
            Deselect();
        }
    }
    void SelectedObject(Transform transform)
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.green;
        ultimoReconozido = transform.gameObject;
    }
    void Deselect()
    {
        if (ultimoReconozido)
        {
            ultimoReconozido.GetComponent<Renderer>().material.color = Color.white;
            ultimoReconozido = null;
        }
    }
    void OnGUI()
    {
        Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
        GUI.DrawTexture(rect, puntero);
        if (ultimoReconozido)
        {
            TextDetect.SetActive(true);
        }
        else
        {
            TextDetect.SetActive(false);
        }
    }
}
