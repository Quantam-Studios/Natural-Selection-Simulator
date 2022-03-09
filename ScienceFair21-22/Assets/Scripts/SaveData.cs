using UnityEngine;
using System.IO;


public class SaveData : MonoBehaviour
{
    public void CreateText(string content)
    {
        // path of the file
        string path = Application.dataPath + "/Log.txt";
        // create file if it doesnt exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Data Logs:\n");
        }
        // add the content
        File.AppendAllText(path, content + "\n");
    }
}
