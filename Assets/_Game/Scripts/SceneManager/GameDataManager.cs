using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
   // Create a field for the save file.

    public static PiecesList ReadFile(string game)
    {
        string saveFile = Application.persistentDataPath + game + ".json";
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            return JsonUtility.FromJson<PiecesList>(fileContents);
        }
        return null;
    }

    public static void WriteFile(PiecesList gameData, string game)
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(gameData);

        // Write JSON to file.
        File.WriteAllText(Application.persistentDataPath + game + ".json", jsonString);
    }
}
