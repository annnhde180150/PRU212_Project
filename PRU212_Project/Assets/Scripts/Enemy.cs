using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("BaseStats")]
    [SerializeField] public float health = 1f;
    [SerializeField] public float damage = 1f;
    [SerializeField] public float speed = 5f;
    [SerializeField] public Transform rangeObject;
    [SerializeField] public float RespawmTime = 5f;
    [SerializeField] public float StuntTime = 3f;
    [SerializeField] public Animator animation;
    public bool isDead = false;
    public bool isStunned = false;
    protected bool canTurn = true;
    protected float range;
    protected int direction = 1;
    protected Rigidbody2D rb;
    protected Vector3 spawnPosition;
    protected bool isRangeReached;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        range = rangeObject.localScale.x / 2f;
        range -= 0.1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected int flip()
    {
        transform.Rotate(0, 180, 0);
        return direction *= -1;
    }

    protected void Move(int direction)
    {
        if(!isStunned)
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    protected void checkRange()
    {
        isRangeReached = rb.position.x > spawnPosition.x + range || rb.position.x < spawnPosition.x - range;
    }

    public void Die()
    {
        isDead = true;
        rb.GetComponent<Collider2D>().enabled = false;
        rb.GetComponent<SpriteRenderer>().enabled = false;
    }

    protected IEnumerator Respawn()
    {
        yield return new WaitForSeconds(RespawmTime);
        transform.position = spawnPosition;
        isDead = false;
        rb.GetComponent<Collider2D>().enabled = true;
        rb.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected IEnumerator Stunt()
    {
        isStunned = true;
        rb.linearVelocity = new Vector2(0, 0);
        animation.SetBool("isStunt", true);
        yield return new WaitForSeconds(StuntTime);
        isStunned = false;
        animation.SetBool("isStunt", false);
    }
}