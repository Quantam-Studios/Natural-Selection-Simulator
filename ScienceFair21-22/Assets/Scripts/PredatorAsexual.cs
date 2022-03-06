using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAsexual : MonoBehaviour
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
    public float energyForRep;

    // CATCHING PREY
    [Header("Catching Prey")]
    public float setRestTime;
    public bool rest;

    // RUN TIME EFFECTED VARIABLES
    [Header("Simulated Variables")]
    public float energy;
    public bool readyForRep;
    private float restTime;
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
    [Header("Sensory Variables")]
    public float senseRadius;
    // check for food
    public bool foodClose;
    public LayerMask food;
    private Vector2 foodObjectPos;

    // REPRODUCTION
    [Header("Reproduction")]
    public GameObject predatorAsexual;
    // parent object holding all spawned offspring
    private Transform parentObjectOfOffspring;
    // Mutations
    [Header("Mutations")]
    public int mutationRate;
    public float minSize;
    public float maxSize;
    public float minSpeed;
    public float maxSpeed;
    public float minSenseRadius;
    public float maxSenseRadius;

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
        // Set restTime
        restTime = setRestTime;
        // Set rest
        rest = false;
        // Update statistics

        // Set parentObjectOfOffspring to the object holding all PREDATOR creatures
        parentObjectOfOffspring = GameObject.FindGameObjectWithTag("PredatorAsexualHolder").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // WANDER STATE
        // this state has the predator wander around the world.
        // In this state the predator can: Move, Rest.
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

        // GETFOOD STATE
        // this state has the predator move to the food piece it sensed.
        // In this state the predator can: Move.
        if (currentState == "GetFood")
        {
            // Set targetPos to the food object.
            targetPos = foodObjectPos;
            // move
            move = true;
        }

        // MOVING / ENERGY CONSUMPTION
        // When move is true AND rest is false, Move towards the targetPos 
        if (move == true && rest == false)
        {
            // Move to targetPos
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // This deals with the constant loss of energy from moving
            // size is now factored in because size effects how much energy is needed to move
            energy -= speed / (1f + size) * Time.timeScale;
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
        // when the swapSides bool is true then start a timer that will wait for the predator to swap sides
        // this allows the predator to never become stuck in an endless state of swapping sides while also allowing normal sensing
        if (swapSides)
        {
            // Inititalize the count down timer
            hitBoundsTime = setHitBoundsTime;
            // Count down
            hitBoundsTime -= Time.deltaTime;
            // If hitBoundsTime hits 0
            if (hitBoundsTime <= 0)
            {
                // Set Predator layer to its original layer
                gameObject.layer = initialLayer;
                // Stop the countdown by setting swapSides to false
                swapSides = false;
            }
        }

        // DEATH FROM LACK OF ENERGY
        // this will run if the energy of the predator ever reaches 0
        if (energy <= 0)
        {
            // Update statistics

            Destroy(gameObject);
        }

        // FORCED REST 
        // this only happens when this predator fails to catch prey (food)
        if (rest == true)
        {
            // start countdown of rest 
            restTime -= Time.deltaTime;
            if (restTime <= 0)
            {
                rest = false;
                // reset rest time
                restTime = setRestTime;
            }
        }
    }

    private void FixedUpdate()
    {
        // SENSORY
        // This happens all of the time no matter what, and controls the states.
        // check for food
        foodClose = Physics2D.OverlapCircle(transform.position, senseRadius, food, 0);

        // DECISION MAKING
        // priorities: #1 Keep energy up
        if (foodClose == true)
        {
            if (foodClose == true)
            {
                if (foodClose == true)
                {
                    GameObject potentialPrey = Physics2D.OverlapCircle(transform.position, senseRadius, food, 0).gameObject;
                    // check size of predator compared to food size
                    // this ensures predators only eat food that is smaller than them
                    // if the sensed prey is smaller then or the same size as the predator (itself) then chase the prey
                    if (potentialPrey.transform.localScale.x <= size)
                    {
                        foodObjectPos = potentialPrey.transform.position;
                        currentState = "GetFood";
                    }
                    else // if not then wander
                    {
                        currentState = "Wander";
                    }
                }
            } // If food is not nearby wander
            else
            {
                currentState = "Wander";
            }
        } // If not then wander
        else
        {
            currentState = "Wander";
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
        // SPEED DETERMINATION
        float offspringSpeed = speed;
        // mutations of speed
        int mutateSpeed = Random.Range(0, mutationRate);
        if (mutateSpeed == 1)
            // then mutate speed
            offspringSpeed = Random.Range(1f, 8f);

        // SIZE DETERMINATION
        float offspringSize = size;
        // mutations of size
        int mutateSize = Random.Range(0, mutationRate);
        if (mutateSize == 1)
            // then mutate size
            offspringSize = Random.Range(0.1f, 1.5f);

        // SENSORY DETERMINATION
        float offspringSenseRadius = senseRadius;
        // mutations of sense radius
        int mutateSenseRadius = Random.Range(0, mutationRate);
        if (mutateSenseRadius == 1)
            // then mutate sense radius
            offspringSize = Random.Range(minSenseRadius, maxSenseRadius);

        // Create Offspring
        GameObject offspring = Instantiate(predatorAsexual, transform.position, Quaternion.identity, parentObjectOfOffspring);
        offspring.GetComponent<PredatorAsexual>().size = offspringSize;
        offspring.GetComponent<PredatorAsexual>().speed = offspringSpeed;
        offspring.GetComponent<PredatorAsexual>().senseRadius = offspringSenseRadius;
        offspring.GetComponent<PredatorAsexual>().energy = 2500;

        // Reset this creatures state, and take away energy
        energy -= energyForRep;
        currentState = "Wander";
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // Collision with food
        if (food == (food | (1 << col.gameObject.layer)))
        {
            // check size of predator compared to food size
            // this ensures predators only wat food that is smaller than them
            if (size >= col.gameObject.transform.localScale.x)
            {
                // catch prey or not
                float sizeDifference = (size -= col.gameObject.transform.localScale.x);
                float chanceToCatch = Random.Range(0f, sizeDifference);

                // if chanceToCatch is greater than 50% of sizeDifference then eat food
                if (chanceToCatch > sizeDifference / 2)
                {
                    // Add energy
                    energy += energyInFood;
                    // destroy the food
                    Destroy(col.gameObject);
                }
                else // Forced Rest
                {
                    rest = true;
                }
            }
        }

        // Collision with periodic bounds
        // Deals with the layers
        // Does NOT deal with moving of the predator
        if (col.gameObject.CompareTag("Periodic"))
        {
            // Set Predator to a layer that can't be interacted with by periodicBounds layer
            gameObject.layer = 11;
            // Start timer to move back to original layer 
            swapSides = true;
        }
    }
}
