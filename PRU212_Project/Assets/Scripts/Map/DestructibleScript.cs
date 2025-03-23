using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleScript : MonoBehaviour
{
    private Tilemap tilemap;
    private HashSet<Vector3Int> visitedTiles;

    public AudioSource audioSource;
    public AudioClip breakSound;
    public GameObject brokenWallPrefab;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        visitedTiles = new HashSet<Vector3Int>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player.isDashing || player.isSpecialAttack)
            {
                Vector2 hitPosition = collision.GetContact(0).point;
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                if (tilemap.HasTile(tilePosition))
                {
                    // Count the number of tiles destroyed
                    int tilesDestroyed = FloodFill(tilePosition);

                    if (breakSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(breakSound);
                    }
                    Debug.Log("Tiles destroyed: " + tilesDestroyed);

                    // Set the number of pieces based on the number of tiles destroyed
                    int numberOfPieces = Mathf.Max(1, tilesDestroyed); 
                    for (int i = 0; i < numberOfPieces; i++)
                    {
                        Vector2 spawnPosition = hitPosition + new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 2f));
                        GameObject broken = Instantiate(brokenWallPrefab, spawnPosition, Quaternion.identity);
                        Rigidbody2D rb = broken.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.freezeRotation = true;
                            Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 5f));
                            rb.AddForce(randomForce, ForceMode2D.Impulse);
                        }

                        Destroy(broken, 3f);
                    }
                }
            }
        }
    }

    private int FloodFill(Vector3Int startTile)
    {
        Queue<Vector3Int> tilesToCheck = new Queue<Vector3Int>();
        tilesToCheck.Enqueue(startTile);
        visitedTiles.Clear();
        int tilesDestroyed = 0; // Counter for the number of tiles destroyed

        while (tilesToCheck.Count > 0)
        {
            Vector3Int currentTile = tilesToCheck.Dequeue();

            if (visitedTiles.Contains(currentTile)) continue;

            visitedTiles.Add(currentTile);

            if (tilemap.HasTile(currentTile))
            {
                tilemap.SetTile(currentTile, null);
                tilesDestroyed++; // Increment the counter for each tile destroyed
            }

            Vector3Int[] neighbors = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0), // Right
                new Vector3Int(-1, 0, 0), // Left
                new Vector3Int(0, 1, 0), // Up
                new Vector3Int(0, -1, 0), // Down
                new Vector3Int(1, 1, 0), // Top-right
                new Vector3Int(-1, 1, 0), // Top-left 
                new Vector3Int(1, -1, 0), // Bottom-right 
                new Vector3Int(-1, -1, 0) // Bottom-left 
            };

            foreach (var direction in neighbors)
            {
                Vector3Int neighborTile = currentTile + direction;

                if (tilemap.HasTile(neighborTile) && !visitedTiles.Contains(neighborTile))
                {
                    tilesToCheck.Enqueue(neighborTile);
                }
            }
        }

        return tilesDestroyed; // Return the total number of tiles destroyed
    }
}