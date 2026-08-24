using System.Collections;
using UnityEngine;

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

    [Header("Frecuencia de Caída")]
    public float tiempoEntreCaidas = 1.2f;

    [Header("Control de Power-Ups")]
    public int maximoPowerUps = 2; // <--- AQUÍ PUEDES CAMBIAR EL LÍMITE (Por defecto 2)
    private int powerUpsGenerados = 0; // Lleva la cuenta de cuántos han caído

    private float cronometro = 0f;
    private bool enLluviaPositiva = false;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= tiempoEntreCaidas)
        {
            GenerarObjeto();
            cronometro = 0f;
        }
    }

    void GenerarObjeto()
    {
        GameObject objetoAElegir;

        if (enLluviaPositiva)
        {
            objetoAElegir = (Random.value > 0.4f) ? prefabZanahoria : prefabZanahoriaDorada;
        }
        else
        {
            objetoAElegir = SeleccionarObjetoAleatorio();
        }

        if (objetoAElegir != null)
        {
            // Si el objeto elegido fue un PowerUp, sumamos 1 al contador
            if (objetoAElegir == prefabPowerUpItem)
            {
                powerUpsGenerados++;
            }

            float posicionX = Random.Range(limiteIzquierdo, limiteDerecho);
            float zPunto = (transformConejo != null) ? transformConejo.position.z : transform.position.z;

            Vector3 posicionGeneracion = new Vector3(posicionX, alturaY, zPunto);
            Instantiate(objetoAElegir, posicionGeneracion, Quaternion.identity);
        }
    }

    GameObject SeleccionarObjetoAleatorio()
    {
        float probabilidad = Random.value;

        // Si ya se generaron los 2 Power-Ups del nivel, redirigimos la probabilidad a una Zanahoria común
        if (probabilidad < 0.15f)
        {
            if (powerUpsGenerados < maximoPowerUps)
            {
                return prefabPowerUpItem; // Solo sale si la cuenta es menor a 2
            }
            else
            {
                return prefabZanahoria; // Si ya salieron 2, sale una zanahoria normal
            }
        }
        else if (probabilidad < 0.60f)
        {
            return prefabZanahoria;
        }
        else if (probabilidad < 0.80f)
        {
            return prefabZanahoriaDorada;
        }
        else
        {
            return prefabPiedra;
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

        tiempoEntreCaidas = 0.3f;

        yield return new WaitForSeconds(duracion);

        tiempoEntreCaidas = tiempoOriginal;
        enLluviaPositiva = false;
    }
}