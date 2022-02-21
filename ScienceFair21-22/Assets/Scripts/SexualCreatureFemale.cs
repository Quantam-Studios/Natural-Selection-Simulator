using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexualCreatureFemale : MonoBehaviour
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
    public float energyForRep;

    // RUN TIME EFFECTED VARIABLES
    [Header("Simulated Variables")]
    public float energy;
    public bool readyForRep;
    public float timeBtwRep;
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
    // check for mates
    public bool matesClose;
    public LayerMask mates;
    private Vector2 mateObjectPos;

    // REPRODUCTION
    public GameObject SexualMale;
    public GameObject SexualFemale;
    public float setTimeBtwRep;
    public Transform parentObjectOfMales;
    public Transform parentObjectOfFemales;

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
        chromosomes[1] = "X";
        // Set timeBtwRep
        timeBtwRep = setTimeBtwRep;
        // Update statistics
        CreatureStatistics.sexualCreatureCount += 1;
        CreatureStatistics.femaleSexualCreatureCount += 1;
        CreatureStatistics.allTimeFemaleCreatureCount += 1;
        CreatureStatistics.allTimeSexualCreatureCount += 1;
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

        // GETMATE STATE
        // this state has the creature move to a potential mate, and reporoduce
        // In this state the creature can: Move, Reproduce.
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
            energy -= (speed / 2);
        }
        else
        {
            // This deals with the constant loss of energy from resting
            energy -= (1 * Time.deltaTime);
        }

        // Can Creature Reproduce?
        timeBtwRep -= Time.deltaTime;
        if (energy >= minEnergyForRep && timeBtwRep <= 0)
            readyForRep = true;
        else
            readyForRep = false;

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
            CreatureStatistics.sexualCreatureCount -= 1;
            CreatureStatistics.femaleSexualCreatureCount -= 1;
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
        // check for mates
        matesClose = Physics2D.OverlapCircle(transform.position, senseRadius, mates, 0);

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
                        if (potentialMate.GetComponent<SexualCreatureMale>().currentState == "GetMate")
                        {
                            mateObjectPos = Physics2D.OverlapCircle(transform.position, senseRadius, mates, 0).transform.position;
                        }
                    }
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
    void reproduce(SexualCreatureMale mateTraits)
    {
        // Decide Sex Of Offspring
        int randChromosome = Random.Range(0, 2);
        string donatedChromosome = mateTraits.chromosomes[randChromosome];

        // FINAL SEX OF OFFSPRING
        string[] offspringChromosomes = new string[2];
        offspringChromosomes[0] = "X";
        offspringChromosomes[1] = donatedChromosome;

        // Determine Speed Of Offspring
        float offspringSpeed = (mateTraits.speed + speed)/2;

        // Determine Size Of Offspring 
        float offspringSize = (mateTraits.size + size)/2;

        // SPAWNING OF NEW CREATURE (offSpring)
        GameObject offspring;
        if (offspringChromosomes[1] == "Y")
        {
            // the offspring should be a MALE
            offspring = Instantiate(SexualMale, transform.position, Quaternion.identity, parentObjectOfMales);
            SexualCreatureMale offspringTraits = offspring.GetComponent<SexualCreatureMale>();
            offspring.GetComponent<SexualCreatureMale>().size = offspringSize;
            offspring.GetComponent<SexualCreatureMale>().speed = offspringSpeed;
        }
        else if(offspringChromosomes[1] == "X")
        {
            // the offspring should be a FEMALE
            offspring = Instantiate(SexualFemale, transform.position, Quaternion.identity, parentObjectOfFemales);
            offspring.GetComponent<SexualCreatureFemale>().size = offspringSize;
            offspring.GetComponent<SexualCreatureFemale>().speed = offspringSpeed;
        }
        energy -= energyForRep;
        currentState = "Wander";
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // If Creature hits another alike Creature and Is in the GetMate State
        if (col.gameObject.tag == "Male" && currentState == "GetMate")
        {
            // Can Creature Reproduce?
            if (readyForRep == true)
            {
                reproduce(col.gameObject.GetComponent<SexualCreatureMale>());
                timeBtwRep = setTimeBtwRep;
            }
        }

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

