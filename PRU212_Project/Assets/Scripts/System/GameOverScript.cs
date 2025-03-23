using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject enemyManager;
    public static GameOverScript Instance;
    private bool isGameOver = false;
    public GameObject player;
    public GameObject firstCheckpoint;
    private PlayerController playerController;
    public static bool isRestarting = false;
    void Awake()
    {
        gameOverPanel.SetActive(false);
        Instance = this;
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            LoadCheckpoint();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);

        //Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false; 
        }

    }

    public void LoadCheckpoint()
    {
        isGameOver = false;
        Time.timeScale = 1;
        player.transform.position = PlayerManager.lastCheckPointPos;
        gameOverPanel.SetActive(false);
        HeathManager.ReloadHealth();
        if (playerController != null)
        {
            playerController.enabled = true; // Re-enable movement
        }

        //respawn all monster
        var spawner = enemyManager.GetComponent<EnemySpawner>();
        foreach(Transform child in spawner.transform) 
            Destroy(child.gameObject);
        spawner.StopRespawning();
        for (int i =0; i < spawner.types.Length; i++)
            StartCoroutine(spawner.Spawn(spawner.types[i], spawner.spawns[i], 0, spawner.ranges[i]));

        Debug.Log("Load Checkpoint");
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;

        if (playerController != null)
        {
            playerController.enabled = false;
        }
        isRestarting = true;
        HeathManager.ReloadHealth();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
    public void BackToMenu()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
  

    public static bool GetIsRestarting()
    {
        return isRestarting;
    }
}
