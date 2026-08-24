using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// La clase DEBE empezar con public class y sus llaves { }
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
    public int metaPuntuacion = 100;

    // AQUÍ ES DONDE SÍ ES VÁLIDO USAR private (Dentro de las llaves de la clase)
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
            if (textoTiempo != null)
            {
                textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString();
            }
        }
        else
        {
            tiempoRestante = 0;
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

        if (textoPuntajeVictoria != null)
            textoPuntajeVictoria.text = puntajeActual.ToString();

        if (textoTiempoVictoria != null)
            textoTiempoVictoria.text = Mathf.CeilToInt(tiempoRestante).ToString();

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

        if (textoPuntajeGameOver != null)
            textoPuntajeGameOver.text = puntajeActual.ToString();

        if (textoTiempoGameOver != null)
            textoTiempoGameOver.text = Mathf.CeilToInt(tiempoRestante).ToString();

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

    void ActualizarUI()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = puntajeActual.ToString();
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