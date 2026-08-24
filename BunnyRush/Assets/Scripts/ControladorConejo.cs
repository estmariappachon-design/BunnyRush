using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControladorConejo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadMovimiento = 7f;
    public float fuerzaSalto = 22f; // Subimos la fuerza inicial para vencer la gravedad alta

    [Header("Ajustes de Gravedad Acelerada")]
    public float multiplicadorSubida = 1.8f; // Acelera la subida para que no sea flotante
    public float multiplicadorCaida = 4.0f;  // Cae rápido y firme al suelo

    [Header("Límites de Pantalla")]
    public float limiteIzquierdo = -5f;
    public float limiteDerecho = 5f;

    [Header("Sprites de Estado")]
    public Sprite spriteNormal;
    public Sprite spriteCaminarDer;
    public Sprite spriteCaminarIzq;
    public Sprite spriteSaltoDer;
    public Sprite spriteSaltoIzq;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private bool enSuelo = false;
    private float inputHorizontal = 0f;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Congelar rotación y movimiento en Z para que no se incline ni se mueva en profundidad
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        if (inputHorizontal > 0.05f)
        {
            mirandoDerecha = true;
        }
        else if (inputHorizontal < -0.05f)
        {
            mirandoDerecha = false;
        }

        ActualizarSprite();

        float posicionXLimitada = Mathf.Clamp(transform.position.x, limiteIzquierdo, limiteDerecho);
        transform.position = new Vector3(posicionXLimitada, transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(inputHorizontal * velocidadMovimiento, rb.linearVelocity.y, 0f);

        // --- SALTO SNAPPIER (SUBIDA Y CAÍDA RÁPIDAS) ---
        if (rb.linearVelocity.y < 0)
        {
            // Cayendo: Gravedad fuerte para aterrizar rápido
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (multiplicadorCaida - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            // Subiendo: Acelera la trayectoria ascendente sin flotar
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (multiplicadorSubida - 1) * Time.fixedDeltaTime;
        }
    }

    void ActualizarSprite()
    {
        if (!enSuelo)
        {
            if (mirandoDerecha)
            {
                if (spriteSaltoDer != null) spriteRenderer.sprite = spriteSaltoDer;
            }
            else
            {
                if (spriteSaltoIzq != null) spriteRenderer.sprite = spriteSaltoIzq;
            }
        }
        else if (inputHorizontal > 0.05f)
        {
            if (spriteCaminarDer != null) spriteRenderer.sprite = spriteCaminarDer;
        }
        else if (inputHorizontal < -0.05f)
        {
            if (spriteCaminarIzq != null) spriteRenderer.sprite = spriteCaminarIzq;
        }
        else
        {
            if (spriteNormal != null) spriteRenderer.sprite = spriteNormal;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        enSuelo = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        enSuelo = false;
    }

    public void MoverHorizontal(float valor)
    {
        inputHorizontal = valor;
    }

    public void Saltar()
    {
        if (enSuelo)
        {
            // Limpia la velocidad vertical antes de impulsar
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }
}