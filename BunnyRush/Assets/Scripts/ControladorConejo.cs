using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControladorConejo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadMovimiento = 7f;
    public float fuerzaSalto = 15f;

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
    }

    void Update()
    {
        // Actualizar orientación según el movimiento táctil
        if (inputHorizontal > 0.05f)
        {
            mirandoDerecha = true;
        }
        else if (inputHorizontal < -0.05f)
        {
            mirandoDerecha = false;
        }

        ActualizarSprite();

        // Restringir la posición X del conejo para que no se salga de pantalla
        float posicionXLimitada = Mathf.Clamp(transform.position.x, limiteIzquierdo, limiteDerecho);
        transform.position = new Vector3(posicionXLimitada, transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        // Aplicar velocidad física lineal en el eje X
        rb.linearVelocity = new Vector3(inputHorizontal * velocidadMovimiento, rb.linearVelocity.y, 0f);
    }

    void ActualizarSprite()
    {
        // Estado: Aire / Salto
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
        // Estado: Suelo en movimiento
        else if (inputHorizontal > 0.05f)
        {
            if (spriteCaminarDer != null) spriteRenderer.sprite = spriteCaminarDer;
        }
        else if (inputHorizontal < -0.05f)
        {
            if (spriteCaminarIzq != null) spriteRenderer.sprite = spriteCaminarIzq;
        }
        // Estado: Reposo (Idle)
        else
        {
            if (spriteNormal != null) spriteRenderer.sprite = spriteNormal;
        }
    }

    // Detección física directa con el suelo
    private void OnCollisionEnter(Collision collision)
    {
        enSuelo = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        enSuelo = false;
    }

    // Métodos llamados por el script BotonControlTactil de la UI
    public void MoverHorizontal(float valor)
    {
        inputHorizontal = valor;
    }

    public void Saltar()
    {
        if (enSuelo)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }
}