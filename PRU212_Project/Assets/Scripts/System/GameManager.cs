using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour, IData
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int coinsToDrop;
    public GameObject coinPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameData gameData)
    {
        this.score = gameData.score;
    }
    public void SaveData(ref GameData gameData)
    {
        gameData.score = this.score;
    }
    public void addScore(int point)
    {
        score += point;
        if (score < 0) score = 0;
        updateScore();
    }
    public void updateScore()
    {
        scoreText.text = score.ToString();
    }
    public void DropCoins(Vector2 position)
    {
        if (score >= 3) coinsToDrop = 3;
        else coinsToDrop = score;
        for (int i = 0; i <= coinsToDrop - 1; i++)
        {
            Vector2 spawnPosition = position + new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 2f));
            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.freezeRotation = true;
                Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 5f));
                rb.AddForce(randomForce, ForceMode2D.Impulse);
            }
            StartCoroutine(EnableCollectionAfterDelay(coin, 0.5f));
            Destroy(coin, 5f);
            Debug.Log("Coin spawned at:" + coin.transform.position);
        }
    }
    public IEnumerator EnableCollectionAfterDelay(GameObject coin, float delay)
    {
        Collider2D coinCollider = coin.GetComponent<Collider2D>();
        coinCollider.enabled = false; // Disable collection initially
        yield return new WaitForSeconds(delay); // Wait for the delay
        coinCollider.enabled = true; // Enable collection
    }

    public int GetCointCoutn()
    {
        return score;
    }
}
