using UnityEngine;

public class SpawnerObjetos : MonoBehaviour
{
    [Header("Referencia al Jugador")]
    public Transform transformConejo; // Objeto del conejo para tomar su Z exacta

    [Header("Prefabs de Caída")]
    public GameObject prefabZanahoria;
    public GameObject prefabPiedra;

    [Header("Límites de Generación (Eje X)")]
    public float limiteIzquierdo = -4f;
    public float limiteDerecho = 4f;
    public float alturaY = 6f;

    [Header("Frecuencia de Caída")]
    public float tiempoEntreCaidas = 1.2f;

    private float cronometro = 0f;

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
        GameObject objetoAElegir = (Random.value > 0.3f) ? prefabZanahoria : prefabPiedra;

        if (objetoAElegir != null)
        {
            float posicionX = Random.Range(limiteIzquierdo, limiteDerecho);

            // Si asignamos el conejo, usamos su Z real (74); de lo contrario usa la Z del Spawner
            float zPunto = (transformConejo != null) ? transformConejo.position.z : transform.position.z;

            Vector3 posicionGeneracion = new Vector3(posicionX, alturaY, zPunto);

            Instantiate(objetoAElegir, posicionGeneracion, Quaternion.identity);
        }
    }
}