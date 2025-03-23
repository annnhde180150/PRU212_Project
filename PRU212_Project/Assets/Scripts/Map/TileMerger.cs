using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode] // Runs outside Play Mode
public class MergeTilemaps : MonoBehaviour
{
    public Tilemap sourceTilemap; // Wall Tilemap
    public Tilemap targetTilemap; // Ground Tilemap

    void Start()
    {
        if (sourceTilemap == null || targetTilemap == null)
        {
            Debug.LogError("Tilemaps not assigned!");
            return;
        }

        // Get all tiles from Wall Tilemap
        BoundsInt bounds = sourceTilemap.cellBounds;
        TileBase[] allTiles = sourceTilemap.GetTilesBlock(bounds);

        // Copy tiles from Wall to Ground
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    targetTilemap.SetTile(tilePosition, tile);
                }
            }
        }

        // Clear & Remove Wall Tilemap
        sourceTilemap.ClearAllTiles();
        DestroyImmediate(sourceTilemap.gameObject); // Works in Edit Mode
    }
}
