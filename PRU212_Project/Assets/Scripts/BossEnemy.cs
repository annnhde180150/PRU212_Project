using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BossEnemy : Enemy
{
    [Header("Attack")]
    [SerializeField] protected GameObject bulletPrefab;
    protected bool isShooting = false;

    [SerializeField] private Transform firePoint;
    public bool isImmuned = false;
    private bool isPatternAttacking = false;
    public bool isMelee = false;
    private bool canMove = true;
    AnimatorStateInfo stateInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = 1;
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
        if (!isPatternAttacking)
        {
            isPatternAttacking = true;
            StartCoroutine(PatternAttacking());
        }
        if (canMove) Move(direction);
        else Stop();
    }

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

    void Enhance()
    {
        animation.SetBool("isEnhancing", true);
    }

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
        Debug.Log(playPos);
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().SetTarget(playPos);

        //end shooting
        animation.SetBool("isShooting", false);
        canMove = true;
    }

    IEnumerator meleeAttack()
    {
        canMove = false;
        animation.SetBool("isAttacking", true);
        yield return null;

        // Continuously update stateInfo inside the loop
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.1f);

        animation.SetBool("isAttacking",false);
        canMove = true;
    }

    private void Stop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    IEnumerator PatternAttacking()
    {
        yield return StartCoroutine(Shielding());
        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(ShootArm());
        //yield return StartCoroutine(ShootArm());
        //yield return StartCoroutine(ShootArm());
        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(meleeAttack());
        yield return new WaitForSeconds(2.0f);

        //finish pattern
        yield return new WaitForSeconds(5.0f);
        canMove = true;
        isPatternAttacking = false;
    }
}
