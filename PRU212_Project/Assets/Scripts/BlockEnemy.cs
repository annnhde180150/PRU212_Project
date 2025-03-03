using UnityEngine;

public class BlockEnemy : Enemy
{
    private float timeDiff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeDiff += Time.deltaTime;
        if(timeDiff >= shotTimeDiff)
        {
            Shoot();
            timeDiff = 0;
        }
    }
}
