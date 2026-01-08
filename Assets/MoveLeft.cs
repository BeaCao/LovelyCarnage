using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager != null)
        {
            transform.Translate(Vector2.left * gameManager.gameSpeed * Time.deltaTime);
        }

        // --- LÓGICA DE DESTRUCCIÓN Y RECICLAJE ---

        // Si es un OBSTÁCULO o LÁGRIMA y se sale, se destruye
        if (transform.position.x < -15 && !gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        // Si es un SUELO y se sale (suponiendo que mide 20 de ancho)
        // Lo movemos 40 unidades a la derecha (2 veces su ancho) para ponerlo a la cola
        else if (transform.position.x < -20 && gameObject.CompareTag("Ground"))
        {
            transform.Translate(Vector2.right * 39.5f); 
        }
    }
}