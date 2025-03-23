using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleScript : MonoBehaviour
{
    private Tilemap tilemap;
    private HashSet<Vector3Int> visitedTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

            print("isSpecialAttack: " + player.isSpecialAttack);
            if (player.isDashing || player.isSpecialAttack)
            {

                Vector3 hitPosition = collision.GetContact(0).point;
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                if (tilemap.HasTile(tilePosition))
                {
                    FloodFill(tilePosition); 
                    Debug.Log("Wall group broken!");
                }

            }
        }
    }

    private void FloodFill(Vector3Int startTile)
    {
        Queue<Vector3Int> tilesToCheck = new Queue<Vector3Int>();
        tilesToCheck.Enqueue(startTile); 
        visitedTiles.Clear(); 

        while (tilesToCheck.Count > 0)
        {
            Vector3Int currentTile = tilesToCheck.Dequeue();

            if (visitedTiles.Contains(currentTile)) continue;

            visitedTiles.Add(currentTile);

            if (tilemap.HasTile(currentTile))
            {
                tilemap.SetTile(currentTile, null); 
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
    }
}
