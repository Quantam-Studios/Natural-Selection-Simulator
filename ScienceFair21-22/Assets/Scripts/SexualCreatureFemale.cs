using UnityEngine;

public class SexualCreatureFemale : MonoBehaviour
{
    // TRAITS
    [Header("Trait Values")]
    public float speed;
    public float size;
    public float senseRadius;
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
    [Header("Sensory Variables")]
    // check for predators
    public bool notSafe;
    public LayerMask predators;
    private Vector2 predatorPos;
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
    public GameObject SexualMale;
    public GameObject SexualFemale;
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
        // Set parentObjectOfOffspring to the object holding all SEXUAL creatures
        parentObjectOfOffspring = GameObject.FindGameObjectWithTag("SexualCreatureHolder").transform;
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
            targetPos = predatorPos;
            // move
            move = true;
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
            // size is now factored in because size effects how much energy is needed to move
            energy -= speed / (1f + size) * Time.timeScale;
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
            Destroy(gameObject);
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
        } // If not then run away from predator.
        else
        {
            // get the opposite position and then set to predatorPos
            predatorPos = Physics2D.OverlapCircle(transform.position, senseRadius, predators, 0).gameObject.transform.position * -1;
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
        if (offspringSpeed< minSpeed)
            offspringSpeed = minSpeed;
        if (offspringSpeed > maxSpeed)
            offspringSpeed = maxSpeed;

        // Determine Size Of Offspring 
        float offspringSize = (mateTraits.size + size)/2;
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
        float offspringSenseRadius = (mateTraits.senseRadius + senseRadius)/2;
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
        if (offspringChromosomes[1] == "Y")
        {
            // the offspring should be a MALE
            offspring = Instantiate(SexualMale, transform.position, Quaternion.identity, parentObjectOfOffspring);
            // give the newly detemrined trait values to the offspring
            offspring.GetComponent<SexualCreatureMale>().size = offspringSize;
            offspring.GetComponent<SexualCreatureMale>().speed = offspringSpeed;
            offspring.GetComponent<SexualCreatureMale>().senseRadius = offspringSenseRadius;
        }
        else if(offspringChromosomes[1] == "X")
        {
            // the offspring should be a FEMALE
            offspring = Instantiate(SexualFemale, transform.position, Quaternion.identity, parentObjectOfOffspring);
            // give the newly detemrined trait values to the offspring
            offspring.GetComponent<SexualCreatureFemale>().size = offspringSize;
            offspring.GetComponent<SexualCreatureFemale>().speed = offspringSpeed;
            offspring.GetComponent<SexualCreatureFemale>().senseRadius = offspringSenseRadius;
        }

        // take away the energy that it takes to reproduce
        energy -= energyForRep;
        // esure the state doesn't become stuck in GetMate
        currentState = "Wander";
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D col)
    {
        // If Creature hits another alike Creature and Is in the GetMate State
        if (col.gameObject.CompareTag("Male") && currentState == "GetMate")
        {
            // Can Creature Reproduce?
            if (readyForRep == true)
            {
                reproduce(col.gameObject.GetComponent<SexualCreatureMale>());
                timeBtwRep = setTimeBtwRep;
            }
        }

        // Collision with food
        if (col.gameObject.CompareTag("Food"))
        {
            // check size of creature compared to food size
            // this ensures cratures only wat food that is smaller than them
            if(size >= 0.3f)
            {
                // Add energy
                energy += energyInFood;
                // destroy the food
                Destroy(col.gameObject);
            }
        }

        // Collision with periodic bounds
        // Deals with the layers
        // Does NOT deal with moving of the creature
        if (col.gameObject.CompareTag("Periodic"))
        {
            // Set Creature to a layer that can't be interacted with by periodicBounds layer
            gameObject.layer = 11;
            // Start timer to move back to original layer 
            swapSides = true;
        }
    }

    // When Destroy() is called on this object
    private void OnDestroy()
    {
        // Update Statistics
        CreatureStatistics.sexualCreatureCount -= 1;
        CreatureStatistics.femaleSexualCreatureCount -= 1;
    }
}

