using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Paneles de Menús")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;
    public GameObject panelPausa;
    public GameObject panelGanadorFinal; // <--- Nuevo panel con la imagen GANADOR

    [Header("Botón de Pausa (UI)")]
    public Button botonPausaUI;

    private bool enPausa = false;
    private bool juegoTerminado = false;

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
        if (panelGanadorFinal != null) panelGanadorFinal.SetActive(false);

        if (botonPausaUI != null) botonPausaUI.enabled = true;
    }

    public void TogglePausa()
    {
        if (juegoTerminado) return;

        enPausa = !enPausa;
        if (panelPausa != null) panelPausa.SetActive(enPausa);

        if (enPausa)
        {
            ObjetoRecolectable.CambiarVisibilidadObjetos(false);
            Time.timeScale = 0f;
        }
        else
        {
            ObjetoRecolectable.CambiarVisibilidadObjetos(true);
            Time.timeScale = 1f;
        }
    }

    public void Derrota()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        BloquearBotonPausa();

        ObjetoRecolectable.LimpiarObjetosEnPantalla();
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victoria()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        BloquearBotonPausa();

        ObjetoRecolectable.LimpiarObjetosEnPantalla();
        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f;
    }

    private void BloquearBotonPausa()
    {
        if (panelPausa != null) panelPausa.SetActive(false);

        if (botonPausaUI != null)
        {
            botonPausaUI.enabled = false;
        }
    }

    public void SiguienteNivel()
    {
        Time.timeScale = 1f;

        // Si estamos en la escena del Nivel 3 (LVL1 3)
        if (SceneManager.GetActiveScene().name.Equals("LVL1 3"))
        {
            // 1. Desactivamos el Spawner para que NO sigan cayendo más objetos
            SpawnerObjetos spawner = GameObject.FindAnyObjectByType<SpawnerObjetos>();
            if (spawner != null)
            {
                spawner.enabled = false;
            }

            // 2. Destruimos cualquier objeto que haya quedado en pantalla
            ObjetoRecolectable.LimpiarObjetosEnPantalla();

            // 3. Ocultamos el panel de Completado y mostramos el de GANADOR
            if (panelVictoria != null) panelVictoria.SetActive(false);
            if (panelGanadorFinal != null) panelGanadorFinal.SetActive(true);
        }
        else
        {
            // En los niveles 1 y 2 pasa al siguiente nivel normalmente
            int indiceActual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(indiceActual + 1);
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        SceneManager.LoadScene("MENU"); // Carga la escena del menú usando su nombre exacto
    }
}