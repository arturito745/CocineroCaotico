using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Interfaz para los estados del tomate
public interface ITomatoState
{
    void EnterState(TomateRodante tomate);
    void UpdateState(TomateRodante tomate);
    void OnCollisionEnter(TomateRodante tomate, Collision2D collision);
}

// Estado "Quieto"
public class TomatoIdleState : ITomatoState
{
    public void EnterState(TomateRodante tomate)
    {
        tomate.rb.linearVelocity = Vector2.zero; // Se detiene
    }

    public void UpdateState(TomateRodante tomate)
    {
        if (Time.time - tomate.startTime > tomate.idleDuration)
        {
            tomate.TransitionToState(new TomatoRollingState());
        }
    }

    public void OnCollisionEnter(TomateRodante tomate, Collision2D collision) { }
}

// Estado "Rodando"
public class TomatoRollingState : ITomatoState
{
    public void EnterState(TomateRodante tomate)
    {
        tomate.SetRandomDirection();
    }

    public void UpdateState(TomateRodante tomate)
    {
        tomate.AvoidPlayer();
        tomate.rb.linearVelocity = tomate.direction * tomate.speed;
    }

    public void OnCollisionEnter(TomateRodante tomate, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            tomate.direction = -tomate.direction;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            tomate.TransitionToState(new TomatoCaughtState());
        }
    }
}

// Estado "Atrapado" (ahora sigue al jugador)
public class TomatoCaughtState : ITomatoState
{
    public void EnterState(TomateRodante tomate)
    {
        tomate.rb.linearVelocity = Vector2.zero; // Detener cualquier movimiento previo
        tomate.rb.bodyType = RigidbodyType2D.Kinematic;
        // Evita que siga rodando con física

        // Deshabilitar colisión para evitar que sea atrapado dos veces
        tomate.gameObject.layer = LayerMask.NameToLayer("Ingrediente");

        // Activar o agregar ItemManager para que siga al jugador
        ItemManager itemManager = tomate.GetComponent<ItemManager>();
        if (itemManager == null)
        {
            itemManager = tomate.gameObject.AddComponent<ItemManager>();
        }
        itemManager.isPickedUp = true;


    }

    public void UpdateState(TomateRodante tomate)
    {
        // No hace nada aquí, porque el movimiento lo maneja ItemManager
    }

    public void OnCollisionEnter(TomateRodante tomate, Collision2D collision) { }
}

// Clase principal del tomate
public class TomateRodante : MonoBehaviour
{
    public float speed = 3f;
    public float followSpeed = 2f; // Velocidad al seguir al jugador
    public float detectionRadius = 2f;
    public float idleDuration = 2f; // Tiempo inicial en estado Quieto

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform player;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float startTime;

    private ITomatoState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetRandomDirection();
        startTime = Time.time;

        TransitionToState(new TomatoIdleState());
    }

    void Update()
    {
        currentState.UpdateState(this);
        CheckPlayerDistance();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void TransitionToState(ITomatoState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetRandomDirection()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void AvoidPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRadius)
        {
            direction = (transform.position - player.position).normalized;
        }
    }
    void CheckPlayerDistance()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectionRadius)
        {
            // Si el jugador está cerca, recalcular la dirección constantemente para alejarse
            direction = (transform.position - player.position).normalized;
        }
        else if (rb.linearVelocity.magnitude < 0.1f)
        {
            // Si se queda atascado, recalcular una nueva dirección aleatoria
            SetRandomDirection();
        }
    }
}
    

