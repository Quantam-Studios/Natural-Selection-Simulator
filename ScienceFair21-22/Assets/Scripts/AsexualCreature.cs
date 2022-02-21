using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsexualCreature : MonoBehaviour
{
    // TRAITS
    [Header("Trait Values")]
    public float speed;
    public float size;

    // ENERGY SETTINGS
    [Header("Energy Settings")]
    public float minEnergyForRep;
    public float minEnergyToBeHungry;
    public float energyInFood;
    public float energyToRep;

    // RUN TIME EFFECTED VARIABLES
    [Header("Simulated Variables")]
    public float energy;
    public bool readyForRep;
    private float timeBtwRep;
    // MOVEMENT 
    public bool move;
    private Vector2 targetPos;
    private float timeBtwDecision;
    public float setTimeBtwDecision;
    // PERIODIC BOUNDS
    public float setHitBoundsTime;
    private float hitBoundsTime;
    public bool swapSides;
    public int initialLayer;

    // MOVEMENT SETTINGS
    [Header("Movement Values")]
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;

    // STATE
    [Header("State Variables")]
    public string currentState;

    // SENSORY
    [Header("Senesory Variables")]
    public float senseRadius;
    // check for predators
    public bool notSafe;
    public LayerMask predators;
    // check for food
    public bool foodClose;
    public LayerMask food;
    private Vector2 foodObjectPos;

    // REPRODUCTION
    public float setTimeBtwRep;
    public GameObject asexualCreature;
    private Transform parentObjectOfOffspring;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize values
        // Set size based on the size gene
        transform.localScale = new Vector3(size, size, size);
        readyForRep = false;
        // allow movement
        move = true;
        // reset timer of decisions
        timeBtwDecision = 0;
        // reset swap side bool
        swapSides = false;
        // Set initialLayer
        initialLayer = gameObject.layer;
        // Set timeBtwRep
        timeBtwRep = setTimeBtwRep;
        // Update statistics
        CreatureStatistics.allTimeAsexualCreatureCount += 1;
        CreatureStatistics.asexualCreatureCount += 1;
        // Set parentObjectOfOffspring to the object holding all ASEXUAL creatures
        parentObjectOfOffspring = GameObject.FindGameObjectWithTag("AsexualCreatureHolder").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // WANDER STATE
        // this state has the creature wander around the world.
        // In this state the creature can: Move, Rest.
        if (currentState == "Wander")
        {
            // Move or Not
            timeBtwDecision -= Time.deltaTime;
            if (timeBtwDecision <= 0)
            {
                // Reset Timer
                timeBtwDecision = setTimeBtwDecision;
                // Randomly Decide What To Do
                int randint = Random.Range(0, 2);
                if (randint == 0)
                {
                    generateRandTargetPos();
                    move = true;
                }
                else
                    move = false;
            }
        }

        // FLEE STATE
        // this state has the creature move away from predators.
        // In this state the creature can: Move.
        if (currentState == "Flee")
        {
            // update targetPos to be the opposite of the predators direction
            // move to targetpos
        }

        // GETFOOD STATE
        // this state has the creature move to the food piece it sensed.
        // In this state the creature can: Move.
        if (currentState == "GetFood")
        {
            // Set targetPos to the food object.
            targetPos = foodObjectPos;
            // move
            move = true;
        }

        // MOVING / ENERGY CONSUMPTION
        // When move is true, Move towards the targetPos 
        if (move == true)
        {
            // Move to targetPos
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // This deals with the constant loss of energy from moving
            energy -= (speed / 2);
        }
        else
        {
            // This deals with the constant loss of energy from resting
            energy -= (1 * Time.deltaTime);
        }

        // REPRODUCTION
        // if ready for replication then call reproduce()
        if (energy >= minEnergyForRep)
        {
            reproduce();
        }

        

        // PERIODIC BOUNDS
        // when the swapSides bool is true then start a timer that will wait for the creature to swap sides
        // this allows the creature to never become stuck in an endless state of swapping sides while also allowing normal sensing
        if (swapSides)
        {
            // Inititalize the count down timer
            hitBoundsTime = setHitBoundsTime;
            // Count down
            hitBoundsTime -= Time.deltaTime;
            // If hitBoundsTime hits 0
            if (hitBoundsTime <= 0)
            {
                // Set Creature layer to its original layer
                gameObject.layer = initialLayer;
                // Stop the countdown by setting swapSides to false
                swapSides = false;
            }
        }

        // DEATH FROM LACK OF ENERGY
        // this will run if the energy of the creature ever reaches 0
        if (energy <= 0)
        {
            // Update statistics
            CreatureStatistics.asexualCreatureCount -= 1;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // SENSORY
        // This happens all of the time no matter what, and controls the states.
        // check for predators
        notSafe = Physics2D.OverlapCircle(transform.position, senseRadius, predators, 0);
        // check for food
        foodClose = Physics2D.OverlapCircle(transform.position, senseRadius, food, 0);

        // DECISION MAKING
        // priorities: #1 Make sure it is safe, #2 Keep energy up
        if (notSafe == false)
        {
            // CHeck if food is close and move towards it if so.
            if (foodClose == true)
            {
                foodObjectPos = Physics2D.OverlapCircle(transform.position, senseRadius, food, 0).transform.position;
                currentState = "GetFood";
            } // If food is not nearby then wander
            else
            {
                currentState = "Wander";
            }
        } // If not then run away from predator.
        else
        {
            currentState = "Flee";
        }
    }

    // Generate new position 
    void generateRandTargetPos()
    {
        // Generate Random Position
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);
        targetPos = new Vector2(randX, randY);
    }

    // Reproduce
    void reproduce()
    {
        // Create traits to passdown
        float offspringSpeed = speed;
        float offspringSize = size;

        // Create Offspring
        GameObject offspring = Instantiate(asexualCreature, transform.position, Quaternion.identity, parentObjectOfOffspring);
        offspring.GetComponent<AsexualCreature>().size = offspringSize;
        offspring.GetComponent<AsexualCreature>().speed = offspringSpeed;
        offspring.GetComponent<AsexualCreature>().energy = 2500;

        // Reset this creatures state, and take away energy
        energy -= energyToRep;
        currentState = "Wander";
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // Collision with food
        if (col.gameObject.tag == "Food")
        {
            // Add energy
            energy += energyInFood;

            // destroy the food
            Destroy(col.gameObject);
        }

        // Collision with periodic bounds
        // Deals with the layers
        // Does NOT deal with moving of the creature
        if (col.gameObject.tag == "Periodic")
        {
            // Set Creature to a layer that can't be interacted with by periodicBounds layer
            gameObject.layer = 11;
            // Start timer to move back to original layer 
            swapSides = true;
        }
    }
}
