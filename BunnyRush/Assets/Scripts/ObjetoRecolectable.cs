using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    public enum TipoObjeto { Zanahoria, Piedra }

    [Header("Configuración del Objeto")]
    public TipoObjeto tipo;
    public int puntos = 10;
    public float tiempoVida = 6f; // Se destruye automáticamente si cae fuera de pantalla

    void Start()
    {
        // Limpieza automática si cae al vacío
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si choca con el conejo
        if (other.CompareTag("Player"))
        {
            ControladorHUD hud = GameObject.FindAnyObjectByType<ControladorHUD>();

            if (hud != null)
            {
                if (tipo == TipoObjeto.Zanahoria)
                {
                    hud.AgregarPuntos(puntos);
                    hud.CargarPowerUp(15f); // Carga un poco la barra
                }
                else if (tipo == TipoObjeto.Piedra)
                {
                    hud.RestarVida();
                }
            }

            Destroy(gameObject); // Destruir el objeto tras el impacto
        }
    }
}