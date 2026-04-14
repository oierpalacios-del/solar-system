using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public CanvasGroup menuCanvas;
    public TMP_Text colecctxt;
    [SerializeField] private GameObject botonSalir;
    [SerializeField] private GameObject botonContinuar;
    public void changeText(int contador)
    {
        colecctxt.text = contador.ToString() + "/9";
    }
    void Start()
    {
        botonSalir.GetComponent<Button>().onClick.AddListener(()=>Application.Quit());
        botonContinuar.GetComponent<Button>().onClick.AddListener(()=>prepareUI());
        menuCanvas.alpha = 0;
        menuCanvas.blocksRaycasts = false;
        Time.timeScale = 1;
    }
    void prepareUI()
    {
        if (Input.GetKey(KeyCode.Escape) && menuCanvas.alpha == 0)
        {
            Time.timeScale = 0;
            menuCanvas.alpha = 1;
            menuCanvas.blocksRaycasts = true;
        }
        else if (Input.GetKey(KeyCode.Space) && menuCanvas.alpha == 1)
        {
            Time.timeScale = 1;
            menuCanvas.alpha = 0;
            menuCanvas.blocksRaycasts = false;
        }
    }
    void Update()
    {
        prepareUI();
    }
}
