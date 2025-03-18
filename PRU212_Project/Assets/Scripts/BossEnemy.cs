using NUnit.Framework.Constraints;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BossEnemy : Enemy
{
    public bool isImmuned = false;
    private bool isPatternAttacking = false;
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
        if(!isPatternAttacking)
        {
            isPatternAttacking = true;
            StartCoroutine(PatternAttacking());
        }
        if(canMove) Move(direction);
    }

    IEnumerator Shielding()
    {
        canMove = false;
        animation.SetBool("isShielded", true);

        // Continuously update stateInfo inside the loop
        do
        {
            stateInfo = animation.GetCurrentAnimatorStateInfo(0);
            yield return null;  // Wait for the next frame
        } while (stateInfo.normalizedTime < 1.0f);
        isImmuned = true;
        animation.SetBool("isShielded", false);
        canMove = true;
    }

    void Enhance()
    {
        animation.SetBool("isEnhancing", true);
    }

    void Shoot()
    {
        animation.SetBool("isShooting", true);
    }

    void meleeAttack()
    {
        animation.SetBool("isAttacking", true);
    }

    IEnumerator PatternAttacking()
    {
        StartCoroutine(Shielding());
        yield return new WaitForSeconds(1.0f);
        //finish pattern
        canMove = true;
        isPatternAttacking = false;
    }
}
