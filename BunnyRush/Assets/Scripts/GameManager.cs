using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Paneles de Menús")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;
    public GameObject panelPausa;

    private bool enPausa = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    public void TogglePausa()
    {
        enPausa = !enPausa;
        if (panelPausa != null) panelPausa.SetActive(enPausa);
        Time.timeScale = enPausa ? 0f : 1f; // Pausa o reanuda el tiempo y las físicas
    }

    public void Derrota()
    {
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victoria()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f; // Congela la escena al mostrar la imagen de completado
    }

    // Lógica para el botón "Siguiente Nivel" dentro del panel de Victoria
    public void SiguienteNivel()
    {
        Time.timeScale = 1f; // Reactiva la velocidad del tiempo
        int indiceActual = SceneManager.GetActiveScene().buildIndex;
        int totalEscenas = SceneManager.sceneCountInBuildSettings;

        if (indiceActual + 1 < totalEscenas)
        {
            SceneManager.LoadScene(indiceActual + 1);
        }
        else
        {
            // Si es el último nivel, puedes mandar al menú principal o reiniciar
            SceneManager.LoadScene(0);
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu(string nombreMenu)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreMenu);
    }
}