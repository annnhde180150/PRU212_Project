using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reSpawn(string type, Vector3 position, float time)
    {
        StartCoroutine(spawn(type, position, time));
    }

    private IEnumerator spawn(string type, Vector3 position, float time)
    {
        GameObject enemy = null;
        var spawnPosition = position;
        switch (type)
        {
            case "Flying":
                enemy = enemies[0];
                break;
            case "Walking":
                enemy = enemies[1];
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(time);
        enemy.GetComponent<Enemy>().spawnPosition = spawnPosition;
        Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
    }
}
