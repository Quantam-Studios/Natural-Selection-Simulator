using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public TrialConfig[] trialQueue;
    public int activeTrialIndex;
    public bool started;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // This Function is called from the BeginSimSetups() function QueueSetupMenu.cs
    public void CreateQueue(int totalSimulations)
    {
        // Create the Queue Array
        trialQueue = new TrialConfig[totalSimulations];
    }

    // Update is called once per frame
    void Update()
    {
        // QUEUE
        // This Makes The Queue Move Through Trials
        if (started == true)
        {
            if (trialQueue[activeTrialIndex].finished)
            {
                activeTrialIndex += 1;
            }
            else
            {
                if (!trialQueue[activeTrialIndex].started)
                {
                    // Begin Instantiation Of Necessary Creatures (Prey)

                    // Begin Instantiation Of Necessary Predators

                    // SetActive Necessary Stats
                    

                    // ensure this never happens more than once
                    trialQueue[activeTrialIndex].started = true;
                }
            }
        }


        if (trialQueue[trialQueue.Length -1].finished == true)
        {
            // stop running the queue
            started = false;
        }
    }
}
