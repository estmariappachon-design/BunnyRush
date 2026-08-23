using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorHUD : MonoBehaviour
{
    [Header("UI Textos y Vidas")]
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoTiempo;
    public Image displayCorazones;

    [Header("Sprites de Corazones")]
    public Sprite sprite3Corazones;
    public Sprite sprite2Corazones;
    public Sprite sprite1Corazon;

    [Header("Barra Power-Up")]
    public Image barraPowerUp;
    public Button botonPowerUp;

    [Header("Configuración del Juego")]
    public float tiempoTotal = 60f; // Tiempo de partida en segundos
    public int metaPuntuacion = 100;

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

        // Contador de Tiempo hacia atrás
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
            // Tiempo Agotado
            tiempoRestante = 0;
            juegoActivo = false;

            if (GameManager.Instance != null)
            {
                if (puntajeActual >= metaPuntuacion)
                    GameManager.Instance.Victoria();
                else
                    GameManager.Instance.Derrota();
            }
        }
    }

    public void AgregarPuntos(int cantidad)
    {
        if (!juegoActivo) return;

        puntajeActual += cantidad;
        ActualizarUI();
    }

    public void RestarVida()
    {
        if (!juegoActivo) return;

        vidasActuales--;
        ActualizarUI();

        if (vidasActuales <= 0)
        {
            juegoActivo = false;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Derrota();
            }
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

    public void ActivarPowerUp()
    {
        if (cargaPowerUp >= 100f)
        {
            cargaPowerUp = 0f;
            if (barraPowerUp != null) barraPowerUp.fillAmount = 0f;
            if (botonPowerUp != null) botonPowerUp.interactable = false;

            Debug.Log("¡Power-Up Activado!");
            // Lógica adicional (ej. invulnerabilidad o multiplicador)
        }
    }

    void ActualizarUI()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = puntajeActual.ToString();
        }

        // Cambio dinámico de la imagen de corazones
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
                    displayCorazones.enabled = false; // Sin corazones
                    break;
            }
        }
    }
}