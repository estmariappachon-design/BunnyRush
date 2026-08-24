using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

        puntajeActual += cantidad;
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

        // Solo habilita el botón si la barra está llena
        if (cargaPowerUp >= 100f && botonPowerUp != null)
        {
            botonPowerUp.interactable = true;
        }
    }

    // Método asignado al evento OnClick() del botón táctil del HUD
    public void ActivarPowerUp()
    {
        if (cargaPowerUp >= 100f && juegoActivo)
        {
            int indiceEscenaActual = SceneManager.GetActiveScene().buildIndex;

            // Restricción: La lluvia de objetos positivos SOLO funciona en Nivel 2 y Nivel 3 (índices 1 y 2 en Build Settings)
            if (indiceEscenaActual >= 1)
            {
                // Reiniciar barra
                cargaPowerUp = 0f;
                if (barraPowerUp != null) barraPowerUp.fillAmount = 0f;
                if (botonPowerUp != null) botonPowerUp.interactable = false;

                // Activar la lluvia en el Spawner
                SpawnerObjetos spawner = GameObject.FindAnyObjectByType<SpawnerObjetos>();
                if (spawner != null)
                {
                    spawner.ActivarLluviaPositiva(duracionLluvia);
                }
            }
            else
            {
                Debug.Log("El Power-Up de lluvia solo se puede activar en Nivel 2 y Nivel 3.");
            }
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





















//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class ControladorHUD : MonoBehaviour
//{
//    [Header("UI Textos y Vidas")]
//    public TextMeshProUGUI textoPuntaje;
//    public TextMeshProUGUI textoTiempo;
//    public Image displayCorazones;

//    [Header("Textos de Paneles Finales")]
//    public TextMeshProUGUI textoPuntajeVictoria;
//    public TextMeshProUGUI textoTiempoVictoria;
//    public TextMeshProUGUI textoPuntajeGameOver;
//    public TextMeshProUGUI textoTiempoGameOver;

//    [Header("Sprites de Corazones")]
//    public Sprite sprite3Corazones;
//    public Sprite sprite2Corazones;
//    public Sprite sprite1Corazon;

//    [Header("Barra Power-Up")]
//    public Image barraPowerUp;
//    public Button botonPowerUp;
//    public float duracionPowerUp = 5f; // Tiempo de invulnerabilidad en segundos

//    [Header("Configuración del Juego")]
//    public float tiempoTotal = 60f;
//    public int metaPuntuacion = 100;

//    private int puntajeActual = 0;
//    private int vidasActuales = 3;
//    private float tiempoRestante;
//    private float cargaPowerUp = 0f;
//    private bool juegoActivo = true;
//    private bool esInvulnerable = false;

//    void Start()
//    {
//        tiempoRestante = tiempoTotal;
//        ActualizarUI();

//        if (botonPowerUp != null)
//            botonPowerUp.interactable = false;
//    }

//    void Update()
//    {
//        if (!juegoActivo) return;

//        if (tiempoRestante > 0)
//        {
//            tiempoRestante -= Time.deltaTime;
//            if (textoTiempo != null)
//            {
//                textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString();
//            }
//        }
//        else
//        {
//            // Se agota el tiempo sin alcanzar la meta
//            tiempoRestante = 0;
//            ProcesarDerrota();
//        }
//    }

//    public void AgregarPuntos(int cantidad)
//    {
//        if (!juegoActivo) return;

//        puntajeActual += cantidad;
//        ActualizarUI();

//        // Si alcanza la meta de puntos -> Victoria
//        if (puntajeActual >= metaPuntuacion)
//        {
//            ProcesarVictoria();
//        }
//    }

//    public void RestarVida()
//    {
//        // Si el Power-Up está activo, ignoramos los impactos negativos
//        if (!juegoActivo || esInvulnerable) return;

//        vidasActuales--;
//        ActualizarUI();

//        if (vidasActuales <= 0)
//        {
//            ProcesarDerrota();
//        }
//    }

//    private void ProcesarVictoria()
//    {
//        juegoActivo = false;

//        // Mostrar solo el valor numérico de puntaje y el tiempo sobrante
//        if (textoPuntajeVictoria != null)
//            textoPuntajeVictoria.text = puntajeActual.ToString();

//        if (textoTiempoVictoria != null)
//            textoTiempoVictoria.text = Mathf.CeilToInt(tiempoRestante).ToString();

//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.Victoria();
//        }
//    }

//    private void ProcesarDerrota()
//    {
//        juegoActivo = false;

//        // Mostrar solo el valor numérico de puntaje y el tiempo transcurrido/restante
//        if (textoPuntajeGameOver != null)
//            textoPuntajeGameOver.text = puntajeActual.ToString();

//        if (textoTiempoGameOver != null)
//            textoTiempoGameOver.text = Mathf.CeilToInt(tiempoRestante).ToString();

//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.Derrota();
//        }
//    }

//    public void CargarPowerUp(float cantidad)
//    {
//        if (!juegoActivo) return;

//        cargaPowerUp = Mathf.Clamp(cargaPowerUp + cantidad, 0f, 100f);

//        if (barraPowerUp != null)
//        {
//            barraPowerUp.fillAmount = cargaPowerUp / 100f;
//        }

//        if (cargaPowerUp >= 100f && botonPowerUp != null)
//        {
//            botonPowerUp.interactable = true;
//        }
//    }

//    // Método asignado al evento OnClick() del botón de Power-Up en UI
//    public void ActivarPowerUp()
//    {
//        if (cargaPowerUp >= 100f && juegoActivo)
//        {
//            // Consumir la carga y desactivar el botón táctil
//            cargaPowerUp = 0f;
//            if (barraPowerUp != null) barraPowerUp.fillAmount = 0f;
//            if (botonPowerUp != null) botonPowerUp.interactable = false;

//            // Iniciar corrutina de invulnerabilidad
//            StartCoroutine(RutinaInvulnerabilidad());
//        }
//    }

//    private IEnumerator RutinaInvulnerabilidad()
//    {
//        esInvulnerable = true;
//        Debug.Log("¡Power-Up Activado: El conejo es invulnerable!");

//        // Espera los segundos configurados sin bloquear el juego
//        yield return new WaitForSeconds(duracionPowerUp);

//        esInvulnerable = false;
//        Debug.Log("Power-Up Finalizado: El conejo vuelve a recibir daño.");
//    }

//    void ActualizarUI()
//    {
//        if (textoPuntaje != null)
//        {
//            textoPuntaje.text = puntajeActual.ToString();
//        }

//        if (displayCorazones != null)
//        {
//            switch (vidasActuales)
//            {
//                case 3:
//                    displayCorazones.sprite = sprite3Corazones;
//                    break;
//                case 2:
//                    displayCorazones.sprite = sprite2Corazones;
//                    break;
//                case 1:
//                    displayCorazones.sprite = sprite1Corazon;
//                    break;
//                default:
//                    displayCorazones.enabled = false;
//                    break;
//            }
//        }
//    }
//}