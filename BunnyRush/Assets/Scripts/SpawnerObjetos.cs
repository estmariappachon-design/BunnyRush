using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerObjetos : MonoBehaviour
{
    [Header("Referencia al Jugador")]
    public Transform transformConejo;

    [Header("Prefabs de Caída")]
    public GameObject prefabZanahoria;
    public GameObject prefabZanahoriaDorada;
    public GameObject prefabPowerUpItem;
    public GameObject prefabPiedra;

    [Header("Límites de Generación (Eje X)")]
    public float limiteIzquierdo = -4f;
    public float limiteDerecho = 4f;
    public float alturaY = 6f;

    [Header("Control de Power-Ups")]
    public int maximoPowerUps = 2;
    private int powerUpsGenerados = 0;

    // Variables internas que variarán según el Nivel
    private float tiempoEntreCaidas = 1.2f;
    private float probabilidadPiedra = 0.15f;
    private float velocidadCaida = 3f; // <--- VELOCIDAD DE CAÍDA POR DEFECTO

    private float cronometro = 0f;
    private bool enLluviaPositiva = false;

    void Start()
    {
        ConfigurarDificultadSegunNivel();
    }

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= tiempoEntreCaidas)
        {
            GenerarObjeto();
            cronometro = 0f;
        }
    }

    void ConfigurarDificultadSegunNivel()
    {
        int indiceEscena = SceneManager.GetActiveScene().buildIndex;

        // Configuración por Escena / Nivel
        switch (indiceEscena)
        {
            case 0: // NIVEL 1
                tiempoEntreCaidas = 1.2f;
                probabilidadPiedra = 0.15f;
                velocidadCaida = 3f; // <--- Caen lento
                break;

            case 1: // NIVEL 2
                tiempoEntreCaidas = 0.9f;
                probabilidadPiedra = 0.30f;
                velocidadCaida = 6f; // <--- Caen el DOBLE de rápido
                break;

            case 2: // NIVEL 3
                tiempoEntreCaidas = 0.6f;
                probabilidadPiedra = 0.45f;
                velocidadCaida = 10f; // <--- Caen SÚPER rápido
                break;

            default:
                velocidadCaida = 5f;
                break;
        }
    }

    void GenerarObjeto()
    {
        GameObject objetoAElegir;

        if (enLluviaPositiva)
        {
            objetoAElegir = prefabZanahoriaDorada;
        }
        else
        {
            objetoAElegir = SeleccionarObjetoAleatorio();
        }

        if (objetoAElegir != null)
        {
            if (objetoAElegir == prefabPowerUpItem)
            {
                powerUpsGenerados++;
            }

            float posicionX = Random.Range(limiteIzquierdo, limiteDerecho);
            float zPunto = (transformConejo != null) ? transformConejo.position.z : transform.position.z;

            Vector3 posicionGeneracion = new Vector3(posicionX, alturaY, zPunto);

            // 1. Instanciar el objeto
            GameObject objetoCreado = Instantiate(objetoAElegir, posicionGeneracion, Quaternion.identity);

            // 2. Aplicar la velocidad de caída según el Nivel usando Rigidbody

            // Si tu proyecto usa Rigidbody 3D:
            Rigidbody rb3D = objetoCreado.GetComponent<Rigidbody>();
            if (rb3D != null)
            {
                rb3D.linearVelocity = new Vector3(0, -velocidadCaida, 0);
            }

            // Si tu proyecto usa Rigidbody 2D (Descomenta estas líneas si es 2D):
            /*
            Rigidbody2D rb2D = objetoCreado.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.linearVelocity = new Vector2(0, -velocidadCaida);
            }
            */
        }
    }

    GameObject SeleccionarObjetoAleatorio()
    {
        float probabilidad = Random.value;

        if (probabilidad < probabilidadPiedra)
        {
            return prefabPiedra;
        }

        if (probabilidad < (probabilidadPiedra + 0.10f))
        {
            if (powerUpsGenerados < maximoPowerUps)
            {
                return prefabPowerUpItem;
            }
            else
            {
                return prefabZanahoria;
            }
        }

        if (Random.value > 0.4f)
        {
            return prefabZanahoria;
        }
        else
        {
            return prefabZanahoriaDorada;
        }
    }

    public void ActivarLluviaPositiva(float duracion)
    {
        StartCoroutine(RutinaLluviaPositiva(duracion));
    }

    private IEnumerator RutinaLluviaPositiva(float duracion)
    {
        enLluviaPositiva = true;
        float tiempoOriginal = tiempoEntreCaidas;

        tiempoEntreCaidas = 0.25f;

        yield return new WaitForSeconds(duracion);

        tiempoEntreCaidas = tiempoOriginal;
        enLluviaPositiva = false;
    }
}