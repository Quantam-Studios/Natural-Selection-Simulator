using UnityEngine;

public class FoodManager : MonoBehaviour
{
    // FOOD SPAWN SETTINGS
    [Header("Food Spawn Settings (Start)")]
    public int foodSourceAmount;
    // min and max (x,y) values for food spawn locations
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    // food spawn timing
    private float timeBtwSpawn;
    public float setTimeBtwSpawn;
    // the good source gameObject
    public GameObject foodSource;

    private Transform foodSourcesHolder;

    // Start is called before the first frame update
    void Start()
    {
        // Set inital values from settings selected by user
        foodSourceAmount = MenuManager.initalFood;
        setTimeBtwSpawn = MenuManager.foodSpawnRate;
        // Initialize food
        foodSourcesHolder = gameObject.transform.GetChild(0);
        generateInitialFood();
        timeBtwSpawn = setTimeBtwSpawn;
    }


    private void Update()
    {
        timeBtwSpawn -= Time.deltaTime;
        if (timeBtwSpawn <= 0)
        {
            // spawn a new piece of food
            generateNewFood();
            // reset the timer
            timeBtwSpawn = setTimeBtwSpawn;
        }
    }

    // Generate Food
    void generateInitialFood()
    {
        for (int i = 0; i < foodSourceAmount; i++)
        {
            // generate random position
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(x, y, 1);    

            // spawn food source
            Instantiate(foodSource, spawnPos, Quaternion.identity, foodSourcesHolder);
        }
    }
    
    // Generate a single new piece of food
    void generateNewFood()
    {
        // generate random position
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(x, y, 1);

        // spawn food source
        Instantiate(foodSource, spawnPos, Quaternion.identity, foodSourcesHolder);
    }
}
