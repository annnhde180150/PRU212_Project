using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BossEnemy : Enemy
{
    public bool isImmuned = false;
    private bool isPatternAttacking = false;
    public bool isMelee = false;
    private bool canMove = true;
    private float latest;
    private float flipCooldown = 1f;
    AnimatorStateInfo stateInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = -1;
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
        canMove = false;
        animation.SetBool("isShooting", true);
        yield return null;

        Debug.Log(bulletPosition.transform.position);
        // Continuously update stateInfo inside the loop
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        yield return new WaitForSeconds(0.1f);
        
        animation.SetBool("isShooting", false);
        var playPos = GameObject.Find("Player").transform.position;
        if (!isShooting) Shoot(playPos);
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
        yield return StartCoroutine(ShootArm());
        yield return StartCoroutine(ShootArm());
        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(meleeAttack());
        yield return new WaitForSeconds(2.0f);

        //finish pattern
        yield return new WaitForSeconds(5.0f);
        canMove = true;
        isPatternAttacking = false;
    }
}
