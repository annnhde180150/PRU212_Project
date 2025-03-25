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
    [SerializeField] public float StuntTime = 3f;
    [SerializeField] public Animator animation;
    public bool isDead = false;
    protected bool isDying = false;
    public bool isStunned = false;
    protected bool isStunning = false;
    protected bool canTurn = true;
    public float range;
    protected int direction = 1;
    protected Rigidbody2D rb;
    public Vector3 spawnPosition;
    protected bool isRangeReached;
    public string type;

    [Header("Audio")]
    [SerializeField] public AudioClip deathSound;
    [SerializeField] public AudioClip stunSound;
    public AudioSource audioSource;

    [Header("Spawning")]
    [SerializeField] protected EnemySpawner enemySpawner;
    [SerializeField] public float RespawmTime = 5f;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        range = rangeObject.localScale.x / 2f;
        range -= 0.1f;
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual int flip()
    {
        transform.Rotate(0, 180, 0);
        return direction *= -1;
    }

    protected void Move(int direction)
    {
        if(!isStunned && !isDead)
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    protected void checkRange()
    {
        isRangeReached = rb.position.x > spawnPosition.x + range || rb.position.x < spawnPosition.x - range;
    }

    public IEnumerator Die()
    {
        if (isDying) yield break;

        animation.SetBool("isStunt", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        isDying = true;
        enemySpawner = GetComponentInParent<EnemySpawner>();
        audioSource.PlayOneShot(deathSound);
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        yield return new WaitForSeconds(deathSound.length);
        enemySpawner.Respawn(type, spawnPosition, RespawmTime, range);
        Destroy(gameObject);
    }

    protected IEnumerator Stunt()
    {
        if (isStunning) yield break;

        audioSource.PlayOneShot(stunSound);
        isStunning = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        animation.SetBool("isStunt", true);
        yield return new WaitForSeconds(StuntTime);
        isStunned = false;
        isStunning = false;
        animation.SetBool("isStunt", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameOver"))
        {
            StartCoroutine(Die());
        }
    }
}