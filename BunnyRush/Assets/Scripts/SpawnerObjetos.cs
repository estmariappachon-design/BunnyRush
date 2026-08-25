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
    public GameObject prefabZanahoriaPodrida;

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
    private float probabilidadZanahoriaPodrida = 0f;
    private float velocidadCaida = 3f;

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
                probabilidadPiedra = 0.25f;
                probabilidadZanahoriaPodrida = 0f; // <--- CERO en el Nivel 1
                velocidadCaida = 3f;
                break;

            case 1: // NIVEL 2
                tiempoEntreCaidas = 0.9f;
                probabilidadPiedra = 0.25f;
                probabilidadZanahoriaPodrida = 0.15f; // <--- Comienzan a salir aquí
                velocidadCaida = 6f;
                break;

            case 2: // NIVEL 3
                tiempoEntreCaidas = 0.6f;
                probabilidadPiedra = 0.20f;
                probabilidadZanahoriaPodrida = 0.10f;
                velocidadCaida = 9f;
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

            GameObject objetoCreado = Instantiate(objetoAElegir, posicionGeneracion, Quaternion.identity);

            Rigidbody rb3D = objetoCreado.GetComponent<Rigidbody>();
            if (rb3D != null)
            {
                rb3D.linearVelocity = new Vector3(0, -velocidadCaida, 0);
            }
        }
    }

    GameObject SeleccionarObjetoAleatorio()
    {
        float probabilidad = Random.value;

        // 1. Evalúa si cae Piedra
        if (probabilidad < probabilidadPiedra)
        {
            return prefabPiedra;
        }

        // 2. Evalúa si cae Zanahoria Podrida (si la probabilidad es 0, omitirá este bloque)
        if (probabilidadZanahoriaPodrida > 0f && probabilidad < (probabilidadPiedra + probabilidadZanahoriaPodrida))
        {
            return prefabZanahoriaPodrida != null ? prefabZanahoriaPodrida : prefabPiedra;
        }

        // 3. Evalúa si cae PowerUp
        if (probabilidad < (probabilidadPiedra + probabilidadZanahoriaPodrida + 0.10f))
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

        // 4. Objetos normales y especiales de premio
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