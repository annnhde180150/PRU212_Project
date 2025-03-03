using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("Flying Stats")]
    [SerializeField] public float flySpeed = 5f;
    [SerializeField] private float heightDiff = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = 1;
        rb.gravityScale = 0;
        type = "Flying";
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
        if (isStunned)
        {
            rb.gravityScale = 1;
            StartCoroutine(Stunt());
        }
        if (!isStunned)
        {
            rb.gravityScale = 0;
            flyingBack();
        }
        if (isDead)
        {
            rb.gravityScale = 1;
            StartCoroutine(Die());
        }
        Move(direction);
    }

    private void flyingBack()
    {
        if (transform.position.y > spawnPosition.y + heightDiff)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -flySpeed);
        }
        else if (transform.position.y < spawnPosition.y - heightDiff)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }
}