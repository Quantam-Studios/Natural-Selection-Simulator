using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorHermaphrodite : MonoBehaviour
{
    // TRAITS
    [Header("Trait Values")]
    public float speed;
    public float size;
    // tracking of traits
    private PredatorStatistics predatorStatistics;
    // trait divisions
    public int speedDiv;
    public int sizeDiv;
    public int senseRadiusDiv;

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
    public float timeBtwRep;
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
    // check for mates
    public bool matesClose;
    public LayerMask mates;
    private Vector2 mateObjectPos;

    // REPRODUCTION
    [Header("Reproduction")]
    public GameObject hermaphrodite;
    public float setTimeBtwRep;
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
        // Get statistics reference
        predatorStatistics = FindObjectOfType<PredatorStatistics>();

        // Set timeBtwRep
        timeBtwRep = setTimeBtwRep;
        // Set restTime
        restTime = setRestTime;
        // Set rest
        rest = false;
        // Update statistics
        // POPULATIONS ones
        PredatorStatistics.predatorHermaphroditeCount += 1;
        PredatorStatistics.allTimePredatorHermaphroditeCount += 1;
        // TRAIT ones
        // Get Divisions
        // size
        sizeDiv = predatorStatistics.getSizeDivision(size);
        // speed
        speedDiv = predatorStatistics.getSpeedDivision(speed);
        // sense radius
        senseRadiusDiv = predatorStatistics.getSenseRadiusDivision(senseRadius);
        // Update Trait Stats
        // size
        CreatureStatistics.sizeDivisionTracker[sizeDiv] += 1;
        // speed
        CreatureStatistics.speedDivisionTracker[speedDiv] += 1;
        // sense radius
        CreatureStatistics.senseRadiusDivisionTracker[senseRadiusDiv] += 1;

        // Set parentObjectOfOffspring to the object holding all PREDATOR creatures
        parentObjectOfOffspring = GameObject.FindGameObjectWithTag("PredatorHermaphroditeHolder").transform;
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
            PredatorStatistics.predatorHermaphroditeCount -= 1;
            Destroy(gameObject);
        }

        // FORCED REST 
        // this only happens when this predator fails to catch prey (food)
        if (rest == true)
        {
            // start countdown of rest 
            restTime -= Time.deltaTime;
            if(restTime <= 0)
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
                else // if not then wander
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
                    // && potentialMate != gameObject checks if the mate in question is not itself.
                    // This is VITAL because Physics2D.OverlapCircle() returns any objects within the specified LayerMask
                    // Meaning it could return the object it is being emitted from.
                    if (potentialMate.GetComponent<PredatorHermaphrodite>().currentState == "GetMate" && potentialMate != gameObject)
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
    void reproduce(PredatorHermaphrodite mateTraits)
    {
        // Determine Speed Of Offspring
        float offspringSpeed = (mateTraits.speed + speed) / 2;
        // determine variation
        float speedVariation = Random.Range((offspringSpeed * -0.2f), (offspringSpeed * 0.2f));
        // apply variation
        offspringSpeed += speedVariation;
        // mutations of speed
        int mutateSpeed = Random.Range(0, mutationRate);
        if (mutateSpeed == 1)
            // then mutate speed
            offspringSpeed = Random.Range(minSpeed, maxSpeed);
        // final check
        // this makes sure the final traits don't go over set maximum or minimum values
        if (offspringSpeed < minSpeed)
            offspringSpeed = minSpeed;
        if (offspringSpeed > maxSpeed)
            offspringSpeed = maxSpeed;

        // Determine Size Of Offspring 
        float offspringSize = (mateTraits.size + size) / 2;
        // determine variation
        float sizeVariation = Random.Range((offspringSize * -0.2f), (offspringSize * 0.2f));
        // apply variation
        offspringSize += sizeVariation;
        // mutations of size
        int mutateSize = Random.Range(0, mutationRate);
        if (mutateSize == 1)
            // then mutate size
            offspringSize = Random.Range(minSize, maxSize);
        // final check
        // this makes sure the final traits don't go over set maximum or minimum values
        if (offspringSize < minSize)
            offspringSize = minSize;
        if (offspringSize > maxSize)
            offspringSize = maxSize;

        // Determine Sense Radius Of Offspring
        float offspringSenseRadius = (mateTraits.senseRadius + senseRadius) / 2;
        // determine variation
        float senseRadiusVariation = Random.Range((offspringSenseRadius * -0.2f), (offspringSenseRadius * 0.2f));
        // apply variation
        offspringSenseRadius += senseRadiusVariation;
        // mutations of sense radius
        int mutateSenseRadius = Random.Range(0, mutationRate);
        if (mutateSenseRadius == 1)
            // then mutate sense radius
            offspringSenseRadius = Random.Range(minSenseRadius, maxSenseRadius);
        // final check
        // this makes sure the final traits don't go over set maximum or minimum values
        if (offspringSenseRadius < minSenseRadius)
            offspringSenseRadius = minSenseRadius;
        if (offspringSenseRadius > maxSenseRadius)
            offspringSenseRadius = maxSenseRadius;

        // SPAWNING OF NEW CREATURE (offSpring)
        GameObject offspring;
        offspring = Instantiate(hermaphrodite, transform.position, Quaternion.identity, parentObjectOfOffspring);
        // give the newly detemrined trait values to the offspring
        offspring.GetComponent<PredatorHermaphrodite>().size = offspringSize;
        offspring.GetComponent<PredatorHermaphrodite>().speed = offspringSpeed;
        offspring.GetComponent<PredatorHermaphrodite>().senseRadius = offspringSenseRadius;

        // take away the energy that it takes to reproduce
        energy -= energyForRep;
        // esure the state doesn't become stuck in GetMate
        currentState = "Wander";
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // If Predator hits another alike Predator and Is in the GetMate State
        if (col.gameObject.CompareTag("PredatorHermaphrodite") && currentState == "GetMate")
        {
            // Can Predator Reproduce?
            if (readyForRep == true)
            {
                reproduce(col.gameObject.GetComponent<PredatorHermaphrodite>());
                timeBtwRep = setTimeBtwRep;
            }
        }

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

    // When Destroy() is called on this object
    private void OnDestroy()
    {
        // Update Statistics
        // Population
        PredatorStatistics.predatorHermaphroditeCount -= 1;
        // Update Trait Stats
        // size
        PredatorStatistics.sizeDivisionTracker[sizeDiv] -= 1;
        // speed
        PredatorStatistics.speedDivisionTracker[speedDiv] -= 1;
        // sense radius
        PredatorStatistics.senseRadiusDivisionTracker[senseRadiusDiv] -= 1;
    }
}
