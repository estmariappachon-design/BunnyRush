using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorHUD : MonoBehaviour
{
    [Header("UI Textos y Vidas")]
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoTiempo;
    public Image displayCorazones;

    [Header("Textos de Paneles Finales")]
    public TextMeshProUGUI textoPuntajeVictoria;
    public TextMeshProUGUI textoTiempoVictoria;
    public TextMeshProUGUI textoPuntajeGameOver;
    public TextMeshProUGUI textoTiempoGameOver;

    [Header("Sprites de Corazones")]
    public Sprite sprite3Corazones;
    public Sprite sprite2Corazones;
    public Sprite sprite1Corazon;

    [Header("Barra Power-Up")]
    public Image barraPowerUp;
    public Button botonPowerUp;
    public float duracionLluvia = 4f;

    [Header("Configuración del Juego")]
    public float tiempoTotal = 60f;
    public int metaPuntuacion = 100; // Define 100, 300 o 500 según el nivel en el Inspector

    private int puntajeActual = 0;
    private int vidasActuales = 3;
    private float tiempoRestante;
    private float cargaPowerUp = 0f;
    private bool juegoActivo = true;

    void Start()
    {
        tiempoRestante = tiempoTotal;
        ActualizarUI();

        if (botonPowerUp != null)
            botonPowerUp.interactable = false;
    }

    void Update()
    {
        if (!juegoActivo) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarTextoTiempo();
        }
        else
        {
            tiempoRestante = 0;
            ActualizarTextoTiempo();
            ProcesarDerrota();
        }
    }

    public void AgregarPuntos(int cantidad)
    {
        if (!juegoActivo) return;

        puntajeActual = Mathf.Max(0, puntajeActual + cantidad);
        ActualizarUI();

        if (puntajeActual >= metaPuntuacion)
        {
            ProcesarVictoria();
        }
    }

    public void RestarVida()
    {
        if (!juegoActivo) return;

        vidasActuales--;
        ActualizarUI();

        if (vidasActuales <= 0)
        {
            ProcesarDerrota();
        }
    }

    private void ProcesarVictoria()
    {
        juegoActivo = false;

        // Muestra el puntaje conseguido vs la meta del nivel en la pantalla final
        if (textoPuntajeVictoria != null)
            textoPuntajeVictoria.text = puntajeActual.ToString() + "/" + metaPuntuacion.ToString();

        if (textoTiempoVictoria != null)
            textoTiempoVictoria.text = FormatearTiempo(tiempoTotal - tiempoRestante);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayVictoria();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Victoria();
        }
    }

    private void ProcesarDerrota()
    {
        juegoActivo = false;

        // Muestra el puntaje conseguido vs la meta del nivel en la pantalla de Game Over
        if (textoPuntajeGameOver != null)
            textoPuntajeGameOver.text = puntajeActual.ToString() + "/" + metaPuntuacion.ToString();

        if (textoTiempoGameOver != null)
            textoTiempoGameOver.text = FormatearTiempo(tiempoTotal - tiempoRestante);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDerrota();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Derrota();
        }
    }

    public void CargarPowerUp(float cantidad)
    {
        if (!juegoActivo) return;

        cargaPowerUp = Mathf.Clamp(cargaPowerUp + cantidad, 0f, 100f);

        if (barraPowerUp != null)
        {
            barraPowerUp.fillAmount = cargaPowerUp / 100f;
        }

        if (cargaPowerUp >= 100f && botonPowerUp != null)
        {
            botonPowerUp.interactable = true;
        }
    }

    private void ActualizarTextoTiempo()
    {
        if (textoTiempo != null)
        {
            textoTiempo.text = FormatearTiempo(tiempoRestante);
        }
    }

    private string FormatearTiempo(float tiempoEnSegundos)
    {
        int minutos = Mathf.FloorToInt(tiempoEnSegundos / 60f);
        int segundos = Mathf.FloorToInt(tiempoEnSegundos % 60f);
        return string.Format("{0:00}:{1:00} s.", minutos, segundos);
    }

    void ActualizarUI()
    {
        // Formato para el marcador principal en pantalla: 0/100, 0/300 o 0/500
        if (textoPuntaje != null)
        {
            textoPuntaje.text = puntajeActual.ToString() + "/" + metaPuntuacion.ToString();
        }

        if (displayCorazones != null)
        {
            switch (vidasActuales)
            {
                case 3:
                    displayCorazones.sprite = sprite3Corazones;
                    break;
                case 2:
                    displayCorazones.sprite = sprite2Corazones;
                    break;
                case 1:
                    displayCorazones.sprite = sprite1Corazon;
                    break;
                default:
                    displayCorazones.enabled = false;
                    break;
            }
        }
    }
}