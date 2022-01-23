using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexualCreature : MonoBehaviour
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

    // RUN TIME EFFECTED VARIABLES
    [Header("Simulated Variables")]
    public float energy;
    public bool readyForRep;
    // MOVEMENT 
    public bool move;
    private Vector2 targetPos;
    private float timeBtwDecision;
    public float setTimeBtwDecision;

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
    // check for mates
    public bool matesClose;
    public LayerMask mates;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize values
        transform.localScale = new Vector3(size, size, size);
        readyForRep = false;
        move = true;
        timeBtwDecision = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // SENSORY
        // This happens all of the time no matter what, and controls the states.
        // check for predators
        notSafe = Physics2D.OverlapCircle(transform.position, senseRadius, predators, 1);
        // check for food
        foodClose = Physics2D.OverlapCircle(transform.position, senseRadius, food, 1);
        // check for mates
        matesClose = Physics2D.OverlapCircle(transform.position, senseRadius, mates, 1);

        // DECISION MAKING
        // priorities: #1 Make sure it is safe, #2 Keep energy up, #3 Mate
        if (notSafe == false)
        {
            // If food or mate is close compare the options
            if (foodClose == true || matesClose == true) 
            {
                // Check if Mating is a viable option or if energy is needed.
                if (energy < minEnergyForRep || energy < minEnergyToBeHungry || !matesClose)
                {
                    foodObjectPos = Physics2D.OverlapCircle(transform.position, senseRadius, food, 1).transform.position;
                    currentState = "GetFood";
                } // If Mating is viable than go to mate.
                else
                {
                    currentState = "GetMate";
                }
            } // If not then wander
            else 
            {
                currentState = "Wander";
            }
        } // If not then run away from predator.
        else 
        {
            currentState = "Flee";
        }

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
            energy -= (speed/2);
        }
        else
        {
            // This deals with the constant loss of energy from resting
            energy -= (1 * Time.deltaTime);
        }

        // Can Creature Reproduce?
        if (energy >= minEnergyForRep)
            readyForRep = true;
        else
            readyForRep = false;    
    }

    // Gnerate new position 
    void generateRandTargetPos()
    {            
        // Generate Random Position
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);
        targetPos = new Vector2(randX, randY);
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // If Creature hits another alike Creature
        if (col.gameObject.tag == "SexRep")
        {
            // Can Creature Reproduce?
            if (readyForRep == true)
            {
                // reproduce();
            }
        }

        // Collision with food
        if(col.gameObject.tag == "Food")
        {
            // Add energy
            energy += energyInFood;

            // destroy the food
            Destroy(col.gameObject);
        }
    }
}
