using UnityEngine;
using UnityEngine.EventSystems;

public class BotonControlTactil : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public enum TipoBoton { Izquierda, Derecha, Salto }

    [Header("Configuración del Botón UI")]
    public TipoBoton tipo;
    public ControladorConejo controlador;

    private bool presionado = false;

    void Update()
    {
        if (presionado && controlador != null)
        {
            if (tipo == TipoBoton.Izquierda)
            {
                controlador.MoverHorizontal(-1f);
            }
            else if (tipo == TipoBoton.Derecha)
            {
                controlador.MoverHorizontal(1f);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        presionado = true;

        if (tipo == TipoBoton.Salto && controlador != null)
        {
            controlador.Saltar();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DetenerAccion();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DetenerAccion();
    }

    private void DetenerAccion()
    {
        presionado = false;
        if ((tipo == TipoBoton.Izquierda || tipo == TipoBoton.Derecha) && controlador != null)
        {
            controlador.MoverHorizontal(0f);
        }
    }
}