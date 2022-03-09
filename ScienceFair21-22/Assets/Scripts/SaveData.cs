using UnityEngine;
using System.IO;


public class SaveData : MonoBehaviour
{
    // Population tracking
    public void CreatePopulationsLog(string content)
    {
        // path of the file
        string path = Application.dataPath + "/PopulationsLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Population Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }


    // Trait tracking
    public void CreateTraitsLog(string content)
    {
        // path of the file
        string path = Application.dataPath + "/TraitsLog.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Trait Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
}
