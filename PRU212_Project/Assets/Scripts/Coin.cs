using UnityEngine;

public class Coin : MonoBehaviour
{
    public string coinID; // Unique ID for each coin

    void Start()
    {
        // Generate a unique ID if not set (useful for editor-generated coins)
        if (string.IsNullOrEmpty(coinID))
        {
            coinID = System.Guid.NewGuid().ToString();
        }

        // Check if this coin was already collected
        if (GameManager.IsCoinCollected(coinID))
        {
            Destroy(gameObject); // Remove the coin from the scene
        }
    }
}
