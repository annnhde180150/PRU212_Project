using System.IO;
using UnityEngine;

public class FileDataHandler
{
   private string _path;
    private string _fileName;

    public FileDataHandler(string path, string fileName)
    {
        _path = path;
        _fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_path, _fileName);
        GameData loadedData = null;

        if (File.Exists(fullPath)) 
        {
            try
            {
                string dataLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataLoad);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load game data: " + e.Message);
            }
        }

        return loadedData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(_path, _fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataJson = JsonUtility.ToJson(gameData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataJson);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
        }
    }
}
