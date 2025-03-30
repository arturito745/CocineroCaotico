using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject Chef;
    public bool isPickedUp;
    private Vector2 vel;
    public float smoothTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            Vector3 offset = new Vector3(0, 1.7f, 0);
            transform.position = Vector2.SmoothDamp(transform.position, Chef.transform.position + offset, ref vel, smoothTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true; 
        }   
    }
}
