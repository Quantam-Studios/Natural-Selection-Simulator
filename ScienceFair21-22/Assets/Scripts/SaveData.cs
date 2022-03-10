using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    string fullPath;

    // Create New Folder
    public void CreateNewLogFolder(string folderName)
    {
        // path of the file
        string path = Application.dataPath + "/" + folderName;
        // create directory if it doesnt exist
        if (!Directory.Exists(path))
        {
            var folder = Directory.CreateDirectory(path);
        }
        // set the path for other functions to reference
        fullPath = path;

        // Create sub folders for prey, and predator data
        string prey = fullPath + "/prey";
        // create directory if it doesnt exist
        if (!Directory.Exists(prey))
        {
            var folder = Directory.CreateDirectory(prey);
        }

        string predators = fullPath + "/predators";
        // create directory if it doesnt exist
        if (!Directory.Exists(predators))
        {
            var folder = Directory.CreateDirectory(predators);
        }
    }

    // LOGGING OF PREY
    // Population tracking
    public void CreatePreyPopulationLog(string content)
    {
        // path of the file
        string path = fullPath + "/prey/PopulationsLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Population Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }

    // size
    public void CreatePreySizeLog(string content)
    {
        // path of the file
        string path = fullPath + "/prey/SizeLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Prey Size Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
    // speed
    public void CreatePreySpeedLog(string content)
    {
        // path of the file
        string path = fullPath + "/prey/SpeedLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Prey Speed Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
    // sense radius
    public void CreatePreySenseRadiusLog(string content)
    {
        // path of the file
        string path = fullPath + "/prey/SenseRadiusLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Prey SenseRadius Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }

    // LOGGING OF PREDATORS
    // Population tracking
    public void CreatePredatorPopulationLog(string content)
    {
        // path of the file
        string path = fullPath + "/predators/PopulationsLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Predator Population Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
    // size
    public void CreatePredatorSizeLog(string content)
    {
        // path of the file
        string path = fullPath + "/predators/SizeLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Predator Size Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
    // speed
    public void CreatePredatorSpeedLog(string content)
    {
        // path of the file
        string path = fullPath + "/predators/SpeedLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Predator Speed Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
    // sense radius
    public void CreatePredatorSenseRadiusLog(string content)
    {
        // path of the file
        string path = fullPath + "/predators/SenseRadiusLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Predator SenseRadius Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
}
