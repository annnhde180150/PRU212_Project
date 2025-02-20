using UnityEngine;

public class WalkingEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = 1;
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        checkRange();
        if (isRangeReached && canTurn)
        {
            direction = flip();
            canTurn = false;
        }
        if (!isRangeReached)
        {
            canTurn = true;
        }
        if(isDead) StartCoroutine(Respawn());
        if (isStunned) StartCoroutine(Stunt());
        Move(direction);
    }
}