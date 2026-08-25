using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    // 1. Agregamos ZanahoriaPodrida al Enum
    public enum TipoObjeto { Zanahoria, ZanahoriaDorada, Piedra, PowerUpItem, ZanahoriaPodrida }

    [Header("Configuración del Objeto")]
    public TipoObjeto tipo;
    public int puntos = 10;
    public float duracionLluviaDorada = 5f;

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
                            AudioManager.Instance.PlayPowerUp();
                            AudioManager.Instance.ActivarMusicaPowerUp(duracionLluviaDorada);
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

                    // 2. Nuevo caso para la Zanahoria Podrida (descuenta puntos y resta vida)
                    case TipoObjeto.ZanahoriaPodrida:
                        hud.RestarVida();
                        hud.AgregarPuntos(-puntos);
                        if (AudioManager.Instance != null) AudioManager.Instance.PlayObjetoMalo();
                        break;
                }
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Suelo") || other.name.Equals("PisoDestructor") || other.name.Equals("Suelo"))
        {
            Destroy(gameObject);
        }
    }

    public static void CambiarVisibilidadObjetos(bool visible)
    {
        ObjetoRecolectable[] objetos = GameObject.FindObjectsByType<ObjetoRecolectable>(FindObjectsInactive.Exclude);
        foreach (ObjetoRecolectable obj in objetos)
        {
            if (obj != null)
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = visible;
                }
            }
        }
    }

    public static void LimpiarObjetosEnPantalla()
    {
        ObjetoRecolectable[] objetos = GameObject.FindObjectsByType<ObjetoRecolectable>(FindObjectsInactive.Exclude);
        foreach (ObjetoRecolectable obj in objetos)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
    }
}