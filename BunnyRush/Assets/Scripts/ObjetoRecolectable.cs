using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    public enum TipoObjeto { Zanahoria, ZanahoriaDorada, Piedra, PowerUpItem }

    [Header("Configuración del Objeto")]
    public TipoObjeto tipo;
    public int puntos = 10;
    public float cargaPowerUp = 25f; // Cuánto carga la barra de UI al atraparlo
    public float tiempoVida = 6f;

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
                        break;

                    case TipoObjeto.ZanahoriaDorada:
                        hud.AgregarPuntos(puntos * 2);
                        break;

                    case TipoObjeto.PowerUpItem:
                        hud.CargarPowerUp(cargaPowerUp); // Súper carga la barra de UI
                        hud.AgregarPuntos(puntos);
                        break;

                    case TipoObjeto.Piedra:
                        hud.RestarVida();
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}














//using UnityEngine;

//public class ObjetoRecolectable : MonoBehaviour
//{
//    // 3 Positivos (Zanahoria, ZanahoriaDorada, EstrellaBonus) y 1 Negativo (Piedra)
//    public enum TipoObjeto { Zanahoria, ZanahoriaDorada, EstrellaBonus, Piedra }

//    [Header("Configuración del Objeto")]
//    public TipoObjeto tipo;
//    public int puntos = 10;
//    public float cargaPowerUp = 15f;
//    public float tiempoVida = 6f;

//    void Start()
//    {
//        Destroy(gameObject, tiempoVida);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            ControladorHUD hud = GameObject.FindAnyObjectByType<ControladorHUD>();

//            if (hud != null)
//            {
//                switch (tipo)
//                {
//                    case TipoObjeto.Zanahoria:
//                        hud.AgregarPuntos(puntos);             // Puntos estándar (10)
//                        hud.CargarPowerUp(cargaPowerUp);       // Carga normal (15%)
//                        break;

//                    case TipoObjeto.ZanahoriaDorada:
//                        hud.AgregarPuntos(puntos * 2);         // Doble de puntos (20)
//                        hud.CargarPowerUp(cargaPowerUp * 2f);  // Carga alta (30%)
//                        break;

//                    case TipoObjeto.EstrellaBonus:
//                        hud.AgregarPuntos(puntos * 5);         // Super puntaje (50)
//                        hud.CargarPowerUp(100f);               // Carga la barra completa de golpe
//                        break;

//                    case TipoObjeto.Piedra:
//                        hud.RestarVida();                      // Quita 1 vida
//                        break;
//                }
//            }

//            Destroy(gameObject);
//        }
//    }
//}