using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BossEnemy : Enemy
{
    public static float damage = 1f;
    [Header("Attack")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform laserPoint;
    [SerializeField] protected GameObject hand;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject laserPrefab;
    [SerializeField] protected AudioClip RampSound;
    [SerializeField] protected AudioClip Enhancing;
    [SerializeField] protected AudioClip Melee;
    [SerializeField] protected AudioClip Hurt;
    [SerializeField] protected AudioClip winning;
    [SerializeField] protected AudioSource mainMusic;
    protected bool isShooting = false;

    [SerializeField] private float laserTime = 4f;
    [SerializeField] private TrailRenderer tr;
    public bool isImmuned = false;
    private bool isPatternAttacking = false;
    public bool isMelee = false;
    private bool canMove = true;
    private bool canStop = true;
    AnimatorStateInfo stateInfo;
    private float offsetX;
    private float offsetY;
    private Coroutine playing;
    private Coroutine action;
    private Vector3 spawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = 1;
        offsetX = _firePoint.position.x - transform.position.x;
        offsetY = _firePoint.position.y - transform.position.y;
        tr.emitting = false;
        tr.enabled = true;
        hand.GetComponent<BoxCollider2D>().enabled = false;
        spawn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
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
            if (!isPatternAttacking)
            {
                isPatternAttacking = true;
                playing = StartCoroutine(PatternAttacking());
            }
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            if (canMove) Move(direction);
            else if (canStop) Stop();
        }
        else StartCoroutine(Die());
    }

    //work
    IEnumerator Shielding()
    {
        canMove = false;
        animation.SetBool("isShielded", true);
        yield return null;

        // Continuously update stateInfo inside the loop
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        audioSource.PlayOneShot(Enhancing);
        yield return new WaitForSeconds(stateInfo.length);
        isImmuned = true;

        animation.SetBool("isShielded", false);
        canMove = true;
    }

    //work
    IEnumerator Rolling()
    {
        canMove = false;
        animation.SetBool("isImmune", true);
        yield return new WaitForSeconds(0.1f);

        // animation
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.22f);

        //rolling
        var playPos = GameObject.Find("Player").transform.position;
        var rollDirection = playPos.x > transform.position.x ? 1 : -1;
        if (direction != rollDirection)
        {
            direction = flip();
        }
        animation.speed = 0;
        audioSource.PlayOneShot(RampSound);
        yield return new WaitForSeconds(1f);
        canStop = false;
        tr.emitting = true;
        rb.linearVelocity = new Vector2(direction * speed * 10f, rb.linearVelocity.y);
        yield return new WaitForSeconds(1f);
        tr.emitting = false;


        //end rolling
        canStop = true;
        animation.SetBool("isImmune", false);
        yield return null;
        animation.speed = 1;
        canMove = true;
    }

    //work
    IEnumerator ShootArm()
    {
        //start shooting
        canMove = false;
        animation.SetBool("isShooting", true);
        yield return null;

        // animation
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.1f);
        
        //render bullet
        var playPos = GameObject.Find("Player").transform.position;
        var newFirePoint = new Vector3(_firePoint.position.x + offsetX*(-direction), _firePoint.position.y - offsetY, 0);
        var bullet = Instantiate(bulletPrefab, newFirePoint, _firePoint.rotation, transform);
        var damage = GetComponent<Enemy>().damage;
        bullet.GetComponent<Bullet>().SetTarget(playPos, _firePoint.position, (int)damage);

        //end shooting
        animation.SetBool("isShooting", false);
        canMove = true;
    }

    //work
    IEnumerator meleeAttack()
    {
        canMove = false;
        isMelee = true;
        animation.SetBool("isMeleeAttack", true);
        hand.GetComponent<BoxCollider2D>().enabled = true;
        yield return null;

        // Continuously update stateInfo inside the loop
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        audioSource.PlayOneShot(Melee);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.5f);

        animation.SetBool("isMeleeAttack", false);
        hand.GetComponent<BoxCollider2D>().enabled = false;
        isMelee = false;
        canMove = true;
    }

    //work
    IEnumerator Stun()
    {
        canMove = false;
        animation.SetBool("isStunt", true);
        isStunned = true;
        isImmuned = false;
        yield return null;

        // Continuously update stateInfo inside the loop
        //stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        //yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(StuntTime);

        animation.SetBool("isStunt", false);
        isStunned = false;
        canMove = true;
    }

    //work
    IEnumerator Lasering()
    {
        canMove = false;
        animation.SetBool("IsLaser", true);
        yield return null;

        var playPos = GameObject.Find("Player").transform.position;
        var laser = Instantiate(laserPrefab, laserPoint.transform.position, _firePoint.rotation, transform);
        laser.GetComponent<Laser>().laserTime = laserTime;
        //bullet.GetComponent<Bullet>().SetTarget(playPos, _firePoint.position);
        yield return new WaitForSeconds(laserTime);

        yield return StartCoroutine(laser.GetComponent<Laser>().End());
        animation.SetBool("IsLaser", false);
        canMove = true;
    }

    //work
    IEnumerator Die()
    {
        if (isDying) yield break;
        if (playing != null)
        {
            StopCoroutine(playing);
            playing = null;
        }
        isDead = true;
        isDying = true;
        canMove = false;

        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
        }

        audioSource.PlayOneShot(deathSound);
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        animation.SetTrigger("IsDying");
        animation.SetBool("isDead", true);
        //yield return new WaitForSeconds(0.8f);

        mainMusic.clip = winning;
        mainMusic.Play();
        DoorScript door = FindFirstObjectByType<DoorScript>();
        if (door != null)
        {
            door.ShowDoor(); 
        }
    }

    private void Stop()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }

    IEnumerator PatternAttacking()
    {
        yield return action = StartCoroutine(Shielding());
        yield return new WaitForSeconds(2.0f);

        yield return action = StartCoroutine(ShootArm());
        yield return action = StartCoroutine(ShootArm());
        yield return action = StartCoroutine(ShootArm());
        yield return new WaitForSeconds(2.0f);

        yield return action = StartCoroutine(meleeAttack());
        yield return new WaitForSeconds(2.0f);

        yield return action = StartCoroutine(Lasering());
        yield return action = StartCoroutine(Rolling());
        yield return action = StartCoroutine(Stun());
        yield return new WaitForSeconds(2.0f);

        //finish pattern
        //yield return new WaitForSeconds(5.0f);
        canMove = true;
        isPatternAttacking = false;
    }

    public void getHurt()
    {
        audioSource.PlayOneShot(Hurt);
    }

    public void reStart()
    {
        transform.position = spawn;
        StopCoroutine(playing);
        StopCoroutine(action);
        isPatternAttacking = false;
        isShooting = false;
        isMelee = false;
        canMove = true;
        canStop = true;
        isImmuned = false;
        health = 10;

        animation.SetBool("IsLaser", false);
        animation.SetBool("isStunt", false);
        animation.SetBool("isMeleeAttack", false);
        animation.SetBool("isShooting", false);
        hand.GetComponent<BoxCollider2D>().enabled = false;
        animation.SetBool("isImmune", false);
        animation.SetBool("isShielded", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Coin")))
        {
            Destroy(collision.gameObject);
        }
    }
}
