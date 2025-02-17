using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Vector2 lastCheckPointPos = Vector2.zero;
    public Collider2D platformCollider;
    private GameOverScript gameOverScript;

    private void Awake()
    {
        lastCheckPointPos = this.gameObject.transform.position;
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
}
