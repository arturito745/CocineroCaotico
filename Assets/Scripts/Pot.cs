using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour
{
    public System.Action<IngredientType> OnIngredientAdded;

    // Referencias a las imágenes de los ingredientes en el Canvas
    [SerializeField] private Image tomatoImage;
    [SerializeField] private Image eggImage;
    [SerializeField] private Image carrotImage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión detectada con: " + other.gameObject.name);

        // Verificar si el objeto que colisionó es un ingrediente
        ItemManager itemManager = other.GetComponent<ItemManager>();
        if (itemManager != null && itemManager.isPickedUp)
        {
            IngredientType type = IngredientType.Tomate; // Por defecto

            // Determinar el tipo de ingrediente
            if (other.GetComponent<TomateRodante>() != null)
            {
                type = IngredientType.Tomate;
                if (tomatoImage != null)
                {
                    tomatoImage.gameObject.SetActive(false); // Ocultar imagen del tomate
                    Debug.Log("Tomate agregado a la olla y eliminado de la receta.");
                }
            }
            else if (other.CompareTag("Huevo"))
            {
                type = IngredientType.Huevo;
                if (eggImage != null)
                {
                    eggImage.gameObject.SetActive(false);
                    Debug.Log("Huevo agregado a la olla y eliminado de la receta.");
                }
            }
            else if (other.CompareTag("Zanahoria"))
            {
                type = IngredientType.Zanahoria;
                if (carrotImage != null)
                {
                    carrotImage.gameObject.SetActive(false);
                    Debug.Log("Zanahoria agregada a la olla y eliminada de la receta.");
                }
            }

            // Disparar el evento
            OnIngredientAdded?.Invoke(type);

            // Destruir el ingrediente en el juego
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("El objeto no es un ingrediente válido o no ha sido recogido.");
        }
    }
}
