using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = -1;
    }

    // Update is called once per frame
    void Update()
    {
        isRangeReached = rb.position.x >= spawnPosition.x + range || rb.position.x <= spawnPosition.x - range;
        if (isRangeReached)
        {
            direction = flip();
        }

        Move(direction);
    }


}
