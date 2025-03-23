using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _filename;
    public static DataManager Instance { get; private set; }
    private static GameData _gameData;
    private List<IData> _dataObjects;
    private FileDataHandler _fileDataHandler;
    private void Awake()
    {
        if (Instance != null)
        {
           Debug.Log("Found more DataManager in the scene.");
        }
        Instance = this;

      
    }

    private void Start()
    {
        string savePath = Application.dataPath + "/../Saves";
        _fileDataHandler = new FileDataHandler(savePath, _filename);
        _dataObjects = FindAllDataObjects();
        LoadGame();

        //Debug.Log(Application.persistentDataPath + " " + _filename);
        Debug.Log(savePath + " " + _filename);
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();
        if (_gameData == null)
        {
            Debug.Log("No game data to load. Initializing new game data");
            NewGame();
        }

        if (GameOverScript.isRestarting)
        {
            GameOverScript.isRestarting = false; 
            Debug.Log("Restarting game");
            NewGame();
        }


        foreach (var dataObject in _dataObjects)
        {
            dataObject.LoadData(_gameData);
        }
        Debug.Log("Loaded coin: " + _gameData.score);
        Debug.Log("Loaded saved at: " + _gameData.checkpointPos);
        Debug.Log("Loaded scene: " + _gameData.currentLevel);
    }

    public void SaveGame()
    {
        if (_gameData == null)
        {
            Debug.Log("No game data to save.");
        }
        foreach (var dataObject in _dataObjects)
        {
            dataObject.SaveData(ref _gameData);
        }
        Debug.Log("Saved coin: " + _gameData.score);
        Debug.Log("Checkpoint saved at: " + _gameData.checkpointPos);
        Debug.Log("Saved scene: " + _gameData.currentLevel);


        _fileDataHandler.Save(_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IData> FindAllDataObjects()
    {
       IEnumerable<IData> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();

        return new List<IData>(dataObjects);
    }

    public GameData GetGameData()
    {
        return _gameData;
    }

    public void SetGameData(GameData newGameData)
    {
        _gameData = newGameData;
    }
}
