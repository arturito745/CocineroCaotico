using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Define los tipos de ingredientes disponibles en el juego
public enum IngredientType
{
    Tomate,
    Huevo,
    Zanahoria
}

// Clase para representar un ingrediente en la receta
[System.Serializable]
public class Ingredient
{
    public IngredientType type;
    public Sprite icon;
    public GameObject prefab;
}

// Interface para el patrón Observer
public interface IRecipeObserver
{
    void OnRecipeProgress(int currentStep, int totalSteps);
    void OnRecipeCompleted();
    void OnRecipeFailed();
}

public class RecipeManager : MonoBehaviour
{
    // Referencias a otros componentes
    [SerializeField] private Pot pot; // Referencia a la olla donde se agregan ingredientes
    [SerializeField] private Transform ingredientSpawnPoints; // Puntos donde aparecen ingredientes

    // Lista de ingredientes disponibles en el juego
    [SerializeField] private List<Ingredient> availableIngredients = new List<Ingredient>();

    // UI Elements
    [SerializeField] private Transform recipeUIContainer; // Contenedor para los iconos de ingredientes
    [SerializeField] private GameObject ingredientIconPrefab; // Prefab para los iconos de la UI
    [SerializeField] private TextMeshProUGUI timerText; // Texto del temporizador
    [SerializeField] private TextMeshProUGUI levelText; // Texto del nivel actual

    // Configuración de nivel
    [SerializeField] private float timePerLevel = 60f; // Tiempo en segundos para completar el nivel
    [SerializeField] private List<Recipe> predefinedRecipes = new List<Recipe>(); // Recetas predefinidas por nivel

    // Variables de estado
    private Queue<IngredientType> currentRecipe = new Queue<IngredientType>();
    private List<GameObject> recipeUIIcons = new List<GameObject>();
    private float remainingTime;
    private int currentLevel = 0;
    private bool levelActive = false;

    // Lista de observadores (patrón Observer)
    private List<IRecipeObserver> observers = new List<IRecipeObserver>();

    // Propiedades públicas
    public int CurrentLevel => currentLevel;
    public float RemainingTime => remainingTime;
    public bool IsLevelActive => levelActive;

    void Start()
    {
        // Suscribir a la olla como receptor de ingredientes
        pot.OnIngredientAdded += ValidateIngredient;

        // Iniciar el primer nivel
        StartLevel(0);
    }

    void Update()
    {
        if (levelActive)
        {
            // Actualizar temporizador
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            // Verificar si se acabó el tiempo
            if (remainingTime <= 0)
            {
                LevelFailed();
            }
        }
    }

    // Inicia un nivel específico
    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= predefinedRecipes.Count)
        {
            Debug.LogError("Nivel inválido: " + levelIndex);
            return;
        }

        // Limpiar estado anterior
        ClearCurrentRecipe();

        // Configurar el nuevo nivel
        currentLevel = levelIndex;
        levelText.text = "Nivel " + (currentLevel + 1);
        remainingTime = timePerLevel;
        levelActive = true;

        // Cargar la receta para este nivel
        LoadRecipe(predefinedRecipes[levelIndex]);

        // Generar los ingredientes en el nivel
        SpawnLevelIngredients();

        // Notificar inicio de nivel
        foreach (var observer in observers)
        {
            observer.OnRecipeProgress(0, currentRecipe.Count);
        }
    }

    // Carga una receta en la cola de ingredientes y actualiza la UI
    private void LoadRecipe(Recipe recipe)
    {
        // Limpiar la cola anterior
        currentRecipe.Clear();

        // Agregar ingredientes a la cola
        foreach (var ingredient in recipe.ingredients)
        {
            currentRecipe.Enqueue(ingredient);
        }

        // Actualizar UI de la receta
        UpdateRecipeUI();
    }

    // Actualiza la UI de la receta mostrando los ingredientes necesarios
    private void UpdateRecipeUI()
    {
        // Limpiar iconos anteriores
        foreach (var icon in recipeUIIcons)
        {
            Destroy(icon);
        }
        recipeUIIcons.Clear();

        // Crear nuevos iconos para cada ingrediente
        IngredientType[] recipeArray = currentRecipe.ToArray();
        for (int i = 0; i < recipeArray.Length; i++)
        {
            GameObject iconObj = Instantiate(ingredientIconPrefab, recipeUIContainer);
            Image iconImage = iconObj.GetComponent<Image>();

            // Buscar el sprite correspondiente al tipo de ingrediente
            Ingredient ingredientData = availableIngredients.Find(ing => ing.type == recipeArray[i]);
            if (ingredientData != null)
            {
                iconImage.sprite = ingredientData.icon;
            }

            // Destacar el primer ingrediente (el que sigue)
            if (i == 0)
            {
                iconImage.color = Color.yellow;
            }

            recipeUIIcons.Add(iconObj);
        }
    }

    // Actualiza el texto del temporizador
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Cambiar color cuando queda poco tiempo
        if (remainingTime < 10)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    // Genera los ingredientes necesarios en el nivel
    private void SpawnLevelIngredients()
    {
        // Contar cuántos ingredientes de cada tipo necesitamos
        Dictionary<IngredientType, int> requiredIngredients = new Dictionary<IngredientType, int>();

        foreach (var ingredient in currentRecipe)
        {
            if (requiredIngredients.ContainsKey(ingredient))
            {
                requiredIngredients[ingredient]++;
            }
            else
            {
                requiredIngredients[ingredient] = 1;
            }
        }

        // Generar al menos los ingredientes necesarios (y algunos extra)
        foreach (var entry in requiredIngredients)
        {
            // Obtener el prefab del ingrediente
            Ingredient ingredientData = availableIngredients.Find(ing => ing.type == entry.Key);
            if (ingredientData != null && ingredientData.prefab != null)
            {
                // Generar la cantidad necesaria + 1 extra
                int countToSpawn = entry.Value + 1;
                for (int i = 0; i < countToSpawn; i++)
                {
                    // Elegir un punto aleatorio entre los spawn points
                    if (ingredientSpawnPoints.childCount > 0)
                    {
                        int randomIndex = Random.Range(0, ingredientSpawnPoints.childCount);
                        Transform spawnPoint = ingredientSpawnPoints.GetChild(randomIndex);

                        // Instanciar el ingrediente
                        Instantiate(ingredientData.prefab, spawnPoint.position, Quaternion.identity);
                    }
                    else
                    {
                        // Si no hay spawn points, generar en posición aleatoria en pantalla
                        Vector2 randomPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
                        Instantiate(ingredientData.prefab, randomPos, Quaternion.identity);
                    }
                }
            }
        }
    }

    // Valida si el ingrediente agregado corresponde al siguiente en la receta
    public void ValidateIngredient(IngredientType addedIngredient)
    {
        if (!levelActive || currentRecipe.Count == 0)
            return;

        // Verificar si el ingrediente es el correcto
        if (currentRecipe.Peek() == addedIngredient)
        {
            // Remover el ingrediente de la receta
            currentRecipe.Dequeue();

            // Actualizar la UI
            UpdateRecipeUI();

            // Notificar progreso
            int remaining = currentRecipe.Count;
            int total = recipeUIIcons.Count + 1; // +1 porque ya quitamos uno
            foreach (var observer in observers)
            {
                observer.OnRecipeProgress(total - remaining, total);
            }

            // Verificar si se completó la receta
            if (currentRecipe.Count == 0)
            {
                LevelCompleted();
            }
        }
        else
        {
            // Ingrediente incorrecto
            // Opciones: penalizar con tiempo, mostrar mensaje, etc.
            Debug.Log("¡Ingrediente incorrecto!");
        }
    }

    // Maneja la finalización exitosa del nivel
    private void LevelCompleted()
    {
        levelActive = false;

        // Notificar a observadores
        foreach (var observer in observers)
        {
            observer.OnRecipeCompleted();
        }

        // Avanzar al siguiente nivel
        if (currentLevel < predefinedRecipes.Count - 1)
        {
            // Opción: Mostrar UI de éxito y luego iniciar siguiente nivel
            StartCoroutine(StartNextLevelAfterDelay(2f));
        }
        else
        {
            // Juego completado
            Debug.Log("¡Juego completado!");
            // Mostrar pantalla de victoria
        }
    }

    // Maneja el fallo del nivel (tiempo agotado)
    private void LevelFailed()
    {
        levelActive = false;

        // Notificar a observadores
        foreach (var observer in observers)
        {
            observer.OnRecipeFailed();
        }

        // Mostrar UI de fallo
        Debug.Log("¡Nivel fallido!");
        // Reiniciar nivel o volver al menú
    }

    // Limpia el estado de la receta actual
    private void ClearCurrentRecipe()
    {
        currentRecipe.Clear();

        foreach (var icon in recipeUIIcons)
        {
            Destroy(icon);
        }
        recipeUIIcons.Clear();
    }

    // Inicia el siguiente nivel después de un retraso
    private IEnumerator StartNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartLevel(currentLevel + 1);
    }

    // Métodos para el patrón Observer
    public void AddObserver(IRecipeObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(IRecipeObserver observer)
    {
        observers.Remove(observer);
    }
}

// Clase para definir una receta
[System.Serializable]
public class Recipe
{
    public string name;
    public List<IngredientType> ingredients = new List<IngredientType>();
}



   