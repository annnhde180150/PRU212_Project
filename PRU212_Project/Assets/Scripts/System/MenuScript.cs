using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        GameData gameData = DataManager.Instance.GetGameData();
        if (gameData == null || string.IsNullOrEmpty(gameData.currentLevel))
        {
            Debug.LogWarning("No save data found! Loading default scene...");
            SceneManager.LoadScene("GameScene"); // Default scene
        }
        else
        {
            SceneManager.LoadScene(gameData.currentLevel);
        }
    }
    public void NewGame()
    {
        GameData newGameData = new GameData();
        DataManager.Instance.SetGameData(newGameData);
        DataManager.Instance.SaveGame();
        SceneManager.LoadScene(newGameData.currentLevel);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
