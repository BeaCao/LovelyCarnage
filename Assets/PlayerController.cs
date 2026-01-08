using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- VARIABLES ---
    [Header("Configuración del Jugador")]
    public float jumpForce = 12f;

    [Header("Skins")]
    public Sprite[] misSkins; // Arrastra tus skins aquí en el Inspector

    private Rigidbody2D rb;
    private bool isGrounded = true;

    void Start()
    {
        Debug.Log("✅ SCRIPT INICIADO: PlayerController listo.");
        rb = GetComponent<Rigidbody2D>();

        // --- SISTEMA DE SKINS ---
        // Recuperamos la skin guardada
        int skinID = PlayerPrefs.GetInt("SkinActiva", 0);

        // Verificamos que el ID existe en tu lista
        if (skinID >= 0 && skinID < misSkins.Length)
        {
            // Cambiamos el dibujo
            GetComponent<SpriteRenderer>().sprite = misSkins[skinID];
            // Aseguramos que el color sea blanco (original)
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Debug.LogWarning("⚠️ Skin ID fuera de rango o lista vacía. Cargando skin por defecto.");
            if (misSkins.Length > 0)
            {
                GetComponent<SpriteRenderer>().sprite = misSkins[0];
            }
        }
    }

    void Update()
    {
        // 1. SALTO NORMAL
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // 2. SALTO VARIABLE (Saltar menos si sueltas la tecla)
        // NOTA: Si usas Unity antiguo y 'linearVelocity' da error, cámbialo por 'velocity'
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    // --- COLISIONES FÍSICAS (Choques sólidos: Suelo, Pinchos, Paredes) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // EL CHIVATO: Nos dice contra qué nos hemos golpeado
        Debug.Log("💥 CHOQUE FÍSICO con: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

        // Lógica del Suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Lógica de Muerte (Obstáculos)
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("💀 ¡MUERTE DETECTADA! Llamando a GameManager...");
            
            // Buscamos al Manager para avisar del Game Over
            GameManager gm = FindAnyObjectByType<GameManager>();
            
            if (gm != null)
            {
                gm.GameOver();
            }
            else
            {
                Debug.LogError("🚨 ERROR: No encuentro el 'GameManager' en la escena. ¿Lo has puesto?");
            }
        }
    }

    // --- RECOLECCIÓN (Cosas que atraviesas: Monedas/Lágrimas) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // EL CHIVATO DE TRIGGERS
        // Si pasas por una lágrima y no sale esto, es que la lágrima no tiene "Is Trigger" marcado.
        Debug.Log("👻 ATRAVESANDO OBJETO: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("Tear"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();

            if (gm != null)
            {
                gm.AddTears(10); // Suma 10 lágrimas
            }

            Destroy(other.gameObject); // Borra la lágrima
        }
    }
}