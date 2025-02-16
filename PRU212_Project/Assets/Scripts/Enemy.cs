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
    protected float range;
    protected int direction = 1;
    protected Rigidbody2D rb;
    protected Vector3 spawnPosition;
    protected bool isRangeReached;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        range = rangeObject.localScale.x/2f;
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
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
}
