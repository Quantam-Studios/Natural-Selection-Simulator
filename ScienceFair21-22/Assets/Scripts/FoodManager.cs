using System.Collections;
using System.Collections.Generic;
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
    // the good source gameObject
    public GameObject foodSource;


    //public float minDistanceApart;
    //private Vector3[] foodSourcePositions;

    private Transform foodSourcesHolder;


    // Start is called before the first frame update
    void Start()
    {
        foodSourcesHolder = this.gameObject.transform.GetChild(0);
        generateFood();
    }

    // Generate Food
    void generateFood()
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
}
