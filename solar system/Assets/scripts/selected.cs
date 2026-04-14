using UnityEngine;

public class interactor : MonoBehaviour
{
    LayerMask mask;
    public float distance = 20f;
    public Texture2D puntero;
    public GameObject TextDetect;
    GameObject ultimoReconozido = null;
    int contadorobj = 0;
    public menu menu;
    public CanvasGroup selectCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("raycastDetect");
        TextDetect.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if (menu.menuCanvas.alpha == 0)
        {
            selectCanvas.alpha = 1;
            selectCanvas.blocksRaycasts = true;
            RaycastHit hitinfo;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, distance, mask))
            {
                Deselect();
                if (hitinfo.collider.tag == "bjetoInteractivo")
                {
                    SelectedObject(hitinfo.transform);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        contadorobj++;
                        menu.changeText(contadorobj);
                    }
                }
            }
            else
            {
                Deselect();
            }
        }
        else
        {
            selectCanvas.alpha = 0;
            selectCanvas.blocksRaycasts = false;
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
