using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleScript : MonoBehaviour
{
    private Tilemap tilemap;
    private BoundsInt destructibleAreaBounds; // Store bounds of destructible area
    public int areaSize = 10; // Set the size of the area (e.g., 3x3 block)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        tilemap = GetComponent<Tilemap>(); // Get the Tilemap component
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with this object
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null && (player.isDashing || player.isSpecialAttack)) 
            {
                // Get the tile position of the hit area
                Vector3 hitPosition = collision.contacts[0].point; // Get the point of collision
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                // Determine the region (a larger area) to be destroyed
                // Adjust size based on your needs (for example, 3x3 block)
                destructibleAreaBounds = new BoundsInt(tilePosition.x - areaSize / 2, tilePosition.y - areaSize / 2, tilePosition.z, areaSize, areaSize, 1);

                // Destroy all tiles within the destructible area
                for (int x = destructibleAreaBounds.xMin; x < destructibleAreaBounds.xMax; x++)
                {
                    for (int y = destructibleAreaBounds.yMin; y < destructibleAreaBounds.yMax; y++)
                    {
                        Vector3Int currentTilePos = new Vector3Int(x, y, destructibleAreaBounds.zMin);

                        // If the Tilemap has a tile at this position, remove it
                        if (tilemap.HasTile(currentTilePos))
                        {
                            tilemap.SetTile(currentTilePos, null); // Remove tile at the current position
                        }
                    }
                }

                Debug.Log("Wall group broken!");
            }
        }
    }
}
