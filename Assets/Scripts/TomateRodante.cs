using UnityEngine;

public class TomateRodante : MonoBehaviour
{
    public float speed = 10f;
    public float detectionRadius = 2f;
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetRandomDirection();
    }

    void Update()
    {
        AvoidPlayer();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    void AvoidPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRadius)
        {
            direction = (transform.position - player.position).normalized; // Se aleja del jugador
        }
    }

    void SetRandomDirection()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Movimiento inicial aleatorio
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Si choca con una pared, cambia de direcci�n
        {
            direction = -direction;
        }
    }
}
