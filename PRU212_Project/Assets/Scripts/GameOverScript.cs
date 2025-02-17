using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject gameOverPanel;
    private bool isGameOver = false;
    public GameObject player;
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void LoadCheckpoint()
    {
        isGameOver = false;
        Time.timeScale = 1;

        player.transform.position = PlayerManager.lastCheckPointPos;
        gameOverPanel.SetActive(false);
    }
    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
    public void BackToMenu()
    {
        isGameOver = false;
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
   
}
