using UnityEngine;
using System.Collections;
public class ZanahoriaEscondida : MonoBehaviour
{
    public float tiempoEscondida = 2f;
    public float tiempoVisible = 3f;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        StartCoroutine(EsconderYReaparecer());
    }

    IEnumerator EsconderYReaparecer()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoVisible);
            spriteRenderer.enabled = false;
            collider2D.enabled = false;

            // Reaparece en una nueva posición aleatoria
            yield return new WaitForSeconds(tiempoEscondida);
            transform.position = new Vector2(Random.Range(-4f, 4f), Random.Range(-3f, 3f));
            spriteRenderer.enabled = true;
            collider2D.enabled = true;
        }
    }
}
