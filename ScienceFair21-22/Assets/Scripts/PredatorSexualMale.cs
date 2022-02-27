using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorSexualMale : MonoBehaviour
{
    // TRAITS
    [Header("Trait Values")]
    public float speed;
    public float size;
    public string[] chromosomes;

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
    // check for food
    public bool foodClose;
    public LayerMask food;
    private Vector2 foodObjectPos;
    // check for mates
    public bool matesClose;
    public LayerMask mates;
    private Vector2 mateObjectPos;

    // REPRODUCTION
    public float setTimeBtwRep;

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
        // Set chromosomes
        chromosomes = new string[2];
        chromosomes[0] = "X";
        chromosomes[1] = "Y";
        // Set timeBtwRep
        timeBtwRep = setTimeBtwRep;
        // Update statistics

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

        // GETMATE STATE
        // this state has the predator move to a potential mate, and reporoduce
        // In this state the predator can: Move, Reproduce.
        if (currentState == "GetMate")
        {
            // Set targetPos to potential mate
            targetPos = mateObjectPos;
            // move
            move = true;
            // IMPORTANT
            // reproduction only occurs on touch.
            // see OnCollisionEnter2D() for the calling of Reproduce()
        }

        // MOVING / ENERGY CONSUMPTION
        // When move is true, Move towards the targetPos 
        if (move == true)
        {
            // Move to targetPos
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // This deals with the constant loss of energy from moving
            // size is now factored in because size effects how much energy is needed to move
            energy -= speed / (1f + size);
        }
        else
        {
            // This deals with the constant loss of energy from resting
            energy -= (1 * Time.deltaTime);
        }

        // Can Predator Reproduce?
        timeBtwRep -= Time.deltaTime;
        if (energy >= minEnergyForRep && timeBtwRep <= 0)
            readyForRep = true;
        else
            readyForRep = false;

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
    }

    private void FixedUpdate()
    {
        // SENSORY
        // This happens all of the time no matter what, and controls the states.
        // check for food
        foodClose = Physics2D.OverlapCircle(transform.position, senseRadius, food, 0);
        // check for mates
        matesClose = Physics2D.OverlapCircle(transform.position, senseRadius, mates, 0);

        // DECISION MAKING
        // priorities: #1 Keep energy up, #2 Mate
        // If food or mate is close compare the options
        if (foodClose == true || matesClose == true)
        {
            // Check if Mating is a viable option or if energy is needed.
            if (energy < minEnergyForRep || energy < minEnergyToBeHungry || !matesClose)
            {
                if (foodClose == true)
                {
                    foodObjectPos = Physics2D.OverlapCircle(transform.position, senseRadius, food, 0).transform.position;
                    currentState = "GetFood";
                }
                else
                {
                    currentState = "Wander";
                }
            } // If Mating is viable than go to mate.
            else
            {
                if (matesClose == true && readyForRep)
                {
                    currentState = "GetMate";
                    GameObject potentialMate = Physics2D.OverlapCircle(transform.position, senseRadius, mates, 0).gameObject;
                    // If the sensed potential mate can also mate then move to the mate.
                    if (potentialMate.GetComponent<PredatorSexualFemale>().currentState == "GetMate")
                    {
                        mateObjectPos = Physics2D.OverlapCircle(transform.position, senseRadius, mates, 0).transform.position;
                    }
                }
                else
                {
                    currentState = "Wander";
                }
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
        energy -= energyToRep;
        currentState = "Wander";
        // IMPORTANT
        // actual sharing of traits, and instantiating of new predators is done in the female predator's reproduce() function.
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // If Predator hits another alike Predator and Is in the GetMate State
        if (col.gameObject.tag == "PredatorFemale" && currentState == "GetMate")
        {
            // Can Predator Reproduce?
            if (readyForRep == true)
            {
                reproduce();
                timeBtwRep = setTimeBtwRep;
            }
        }

        // Collision with food
        if (col.gameObject.tag == "AsexualCreature")
        {
            // check size of predator compared to food size
            // this ensures cratures only wat food that is smaller than them
            if (size > 0.3f)
            {
                // Add energy
                energy += energyInFood;
                // destroy the food
                Destroy(col.gameObject);
            }
        }

        // Collision with periodic bounds
        // Deals with the layers
        // Does NOT deal with moving of the predator
        if (col.gameObject.tag == "Periodic")
        {
            // Set Predator to a layer that can't be interacted with by periodicBounds layer
            gameObject.layer = 11;
            // Start timer to move back to original layer 
            swapSides = true;
        }
    }
}
