using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public static Dictionary<string, Vector2> sceneCheckpoints = new Dictionary<string, Vector2>()
    {
        {"GameScene", new Vector2(-37.68f, -3.01f)},
        {"GameScene_2", new Vector2(-9.7f, -3.6f)},
        {"GameScene_3", new Vector2(-7.3f, -2.5f)}
    };
    public static Vector2 GetCheckpointForScene(string sceneName)
    {
        if (sceneCheckpoints.ContainsKey(sceneName))
        {
            return sceneCheckpoints[sceneName];
        }
        return Vector2.zero; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("CheckPoint Reached");
            PlayerManager.lastCheckPointPos = transform.position; 
        }
    }

  
}
