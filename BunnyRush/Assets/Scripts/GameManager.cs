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
        Time.timeScale = enPausa ? 0f : 1f;
    }

    public void Derrota()
    {
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victoria()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f;
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