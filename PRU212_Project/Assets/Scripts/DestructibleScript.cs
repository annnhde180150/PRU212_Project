using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleScript : MonoBehaviour
{
    private Tilemap tilemap;
    private BoundsInt areaBounds; 
    public int areaSize = 10; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        tilemap = GetComponent<Tilemap>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null && (player.isDashing || player.isSpecialAttack)) 
            {

                Vector3 hitPosition = collision.GetContact(0).point;
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                areaBounds = new BoundsInt(tilePosition.x - areaSize / 2, tilePosition.y - areaSize / 2, tilePosition.z, areaSize, areaSize, 1);

                for (int x = areaBounds.xMin; x < areaBounds.xMax; x++)
                {
                    for (int y = areaBounds.yMin; y < areaBounds.yMax; y++)
                    {
                        Vector3Int currentTilePos = new Vector3Int(x, y, areaBounds.zMin);

                      
                        if (tilemap.HasTile(currentTilePos))
                        {
                            tilemap.SetTile(currentTilePos, null); 
                        }
                    }
                }

                Debug.Log("Wall broken");
            }
        }
    }
}
