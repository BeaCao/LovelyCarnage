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

    void Awake()
    {
        // Obtenemos las referencias necesarias
        rb = GetComponent<Rigidbody2D>();
        Animator anim = GetComponent<Animator>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // 1. OCULTAMOS el renderizador inmediatamente para que no se vea el "salto" de skin
        if (sr != null) sr.enabled = false;

        Debug.Log("✅ SCRIPT INICIADO: PlayerController listo.");

        // --- NUEVA LÓGICA DE ANIMATOR ---
        // Recuperamos la skin guardada en la tienda
        int skinID = PlayerPrefs.GetInt("SkinActiva", 0);

        if (anim != null)
        {
            anim.SetInteger("SkinID", skinID);
            // 2. FORZAMOS la actualización del Animator ahora mismo para que elija la skin correcta
            anim.Update(0f);
        }

        // --- SISTEMA DE SKINS (EXISTENTE) ---
        if (skinID >= 0 && skinID < misSkins.Length)
        {
            sr.sprite = misSkins[skinID];
            sr.color = Color.white;
        }
        else
        {
            Debug.LogWarning("⚠️ Skin ID fuera de rango o lista vacía. Cargando skin por defecto.");
            if (misSkins.Length > 0)
            {
                sr.sprite = misSkins[0];
            }
        }

        // 3. MOSTRAMOS el personaje ya con la skin y animación cargadas
        if (sr != null) sr.enabled = true;
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
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    // --- COLISIONES FÍSICAS (Suelo, Obstáculos) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("💥 CHOQUE FÍSICO con: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("💀 ¡MUERTE DETECTADA! Llamando a GameManager...");
            GameManager gm = FindAnyObjectByType<GameManager>();

            if (gm != null)
            {
                gm.GameOver();
            }
            else
            {
                Debug.LogError("🚨 ERROR: No encuentro el 'GameManager' en la escena.");
            }
        }
    }

    // --- RECOLECCIÓN (Lágrimas) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("👻 ATRAVESANDO OBJETO: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("Tear"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();

            if (gm != null)
            {
                gm.AddTears(10);
            }

            Destroy(other.gameObject);
        }
    }
}