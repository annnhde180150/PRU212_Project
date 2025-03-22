using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BossEnemy : Enemy
{
    [Header("Attack")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] protected GameObject bulletPrefab;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = 1;
        offsetX = _firePoint.position.x - transform.position.x;
        offsetY = _firePoint.position.y - transform.position.y;
        tr.emitting = false;
        tr.enabled = true;
        isDead = true;
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
                StartCoroutine(PatternAttacking());
            }
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            if (canMove) Move(direction);
            else if (canStop) Stop();
        }else StartCoroutine(Die());
    }

    //work
    IEnumerator Shielding()
    {
        canMove = false;
        animation.SetBool("isShielded", true);
        yield return null;

        // Continuously update stateInfo inside the loop
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
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

    //work partly
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
        var newFirePoint = new Vector3(_firePoint.position.x + offsetX, _firePoint.position.y + offsetY - 1, 0);
        var bullet = Instantiate(bulletPrefab, newFirePoint, _firePoint.rotation);
        bullet.GetComponent<Bullet>().SetTarget(playPos, _firePoint.position);

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
        yield return null;

        // Continuously update stateInfo inside the loop
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.1f);

        animation.SetBool("isMeleeAttack", false);
        isMelee = false;
        canMove = true;
    }

    //work
    IEnumerator Stun()
    {
        canMove = false;
        animation.SetBool("isStunt", true);
        isImmuned = false;
        yield return null;

        // Continuously update stateInfo inside the loop
        //stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        //yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(StuntTime);

        animation.SetBool("isStunt", false);
        canMove = true;
    }

    IEnumerator Lasering()
    {
        canMove = false;
        animation.SetBool("IsLaser", true);
        yield return null;

        yield return new WaitForSeconds(laserTime);

        animation.SetBool("IsLaser", false);
        canMove = true;
    }

    //not work
    IEnumerator Die()
    {
        if (isDying) yield break;
        isDying = true;
        canMove = false;
        animation.SetBool("isDead", true);
        yield return null;

        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;
        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
        }

        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.5f);
        animation.speed = 0;
    }

    private void Stop()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }

    IEnumerator PatternAttacking()
    {
        yield return StartCoroutine(Shielding());
        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(ShootArm());
        yield return StartCoroutine(ShootArm());
        yield return StartCoroutine(ShootArm());
        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(meleeAttack());
        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(Lasering());
        yield return StartCoroutine(Rolling());
        yield return StartCoroutine(Stun());
        yield return new WaitForSeconds(2.0f);

        //finish pattern
        //yield return new WaitForSeconds(5.0f);
        canMove = true;
        isPatternAttacking = false;
    }
}
