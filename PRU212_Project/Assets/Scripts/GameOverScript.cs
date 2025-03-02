using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject enemyManager;
    private bool isGameOver = false;
    public GameObject player;
    void Awake()
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
        var spawner = enemyManager.GetComponent<EnemySpawner>();
        for (int i =0; i < enemyManager.transform.childCount; i++)
        {
            var enemy = enemyManager.transform.GetChild(i).GetComponent<Enemy>();
            string type = enemy.type;
            Vector3 position = enemy.spawnPosition;
            Destroy(enemy.gameObject);
            StartCoroutine(spawner.spawn(type, position, 0));
        }
        Debug.Log("Load Checkpoint");
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
        SceneManager.LoadScene("MainMenu");
    }
   
}
