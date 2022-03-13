using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueSetupMenu : MonoBehaviour
{
    // for keeping track of the total simulations to run
    public int simulationsToRun;

    // Total Simulation Types
    // how many unique settings need to be made
    public int totalSimulationTypes;
    // Trials Per Simulation
    // how many of each simulation will be run
    public int trialsPerSimulation;

    // QUEUE MANAGER
    public QueueManager queueManager;


    private void Update()
    {

    }


    public void BeginSimSetups()
    {
        // Determine the total simulation count
        simulationsToRun = trialsPerSimulation * totalSimulationTypes;

        // Create The Queue
        queueManager.CreateQueue(simulationsToRun);
    }
}
