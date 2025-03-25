using UnityEngine;

public class PlayerManager : MonoBehaviour, IData
{
    public static Vector2 lastCheckPointPos = Vector2.zero;
    private GameOverScript gameOverScript;

    private void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverScript = FindObjectOfType<GameOverScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GameOver"))
        {
            Debug.Log("Game Over");
            gameOverScript.GameOver();
        }


    }

    public void LoadData(GameData gameData)
    {
        lastCheckPointPos = gameData.checkpointPos;
        transform.position = lastCheckPointPos;
    }
    public void SaveData(ref GameData gameData)
    {
         gameData.checkpointPos = lastCheckPointPos;
    }
}
