using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    public enum TipoObjeto { Zanahoria, ZanahoriaDorada, Piedra, PowerUpItem }

    [Header("Configuración del Objeto")]
    public TipoObjeto tipo;
    public int puntos = 10;
    public float tiempoVida = 6f;
    public float duracionLluviaDorada = 5f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ControladorHUD hud = GameObject.FindAnyObjectByType<ControladorHUD>();

            if (hud != null)
            {
                switch (tipo)
                {
                    case TipoObjeto.Zanahoria:
                        hud.AgregarPuntos(puntos);
                        if (AudioManager.Instance != null) AudioManager.Instance.PlayObjetoBueno();
                        break;

                    case TipoObjeto.ZanahoriaDorada:
                        hud.AgregarPuntos(puntos * 2);
                        if (AudioManager.Instance != null) AudioManager.Instance.PlayObjetoBueno();
                        break;

                    case TipoObjeto.PowerUpItem:
                        hud.AgregarPuntos(puntos);
                        if (AudioManager.Instance != null)
                        {
                            AudioManager.Instance.PlayPowerUp(); // Sonidito instantáneo al recoger
                            AudioManager.Instance.ActivarMusicaPowerUp(duracionLluviaDorada); // Cambia la música por la duración de la lluvia
                        }

                        SpawnerObjetos spawner = GameObject.FindAnyObjectByType<SpawnerObjetos>();
                        if (spawner != null)
                        {
                            spawner.ActivarLluviaPositiva(duracionLluviaDorada);
                        }
                        break;
                    case TipoObjeto.Piedra:
                        hud.RestarVida();
                        hud.AgregarPuntos(-puntos);
                        if (AudioManager.Instance != null) AudioManager.Instance.PlayObjetoMalo();
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}