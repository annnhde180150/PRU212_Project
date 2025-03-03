using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]public GameObject[] enemies;
    public string[] types;
    public Vector3[] spawns;
    private List<Coroutine> respawners = new List<Coroutine>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        types = new string[transform.childCount];
        spawns = new Vector3[transform.childCount];
        for (int i = 0; i < types.Length; i++)
        {
            types[i] = transform.GetChild(i).GetComponent<Enemy>().type;
            spawns[i] = transform.GetChild(i).GetComponent<Enemy>().spawnPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopRespawning()
    {
        foreach(Coroutine coroutine in respawners) StopCoroutine(coroutine);
        respawners.Clear();
    }

    public void Respawn(string type, Vector3 position, float time)
    {
        Coroutine newSpawn =  StartCoroutine(Spawn(type, position, time));
        respawners.Add(newSpawn);
    }

    public IEnumerator Spawn(string type, Vector3 position, float time)
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
            case "Block":
                enemy = enemies[2];
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(time);
        enemy.GetComponent<Enemy>().spawnPosition = spawnPosition;
        Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
    }
}
