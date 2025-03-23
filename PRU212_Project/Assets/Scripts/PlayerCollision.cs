using System.Collections;
using UnityEditor.Search;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController player;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = FindAnyObjectByType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            gameManager.addScore(1);
        }
        //if(collision.CompareTag("Enemy"))
        //{
        //    Debug.Log("Hit enemy");
        //    var enemy = collision.GetComponent<Enemy>();
        //    enemy.isStunned = true;
        //    //gameManager.addScore(-1);
        //    //enemy.Die();
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            var enemy = collision.collider.GetComponent<Enemy>();
            var gameOver = FindAnyObjectByType<GameOverScript>();
            if (player.isSpecialAttack)
            {
                enemy.isDead = true;
                //gameManager.addScore(1);
            }
            else
            if (!enemy.isDead) 
            {
                HeathManager.TakeDamage(1);
                PlayerController.playHurtsound();
                if (HeathManager.health <= 0)
                {
                    gameOver.GameOver();
                }
                else
                {
                    StartCoroutine(GetHurt());
                }
                    
            } 
            else 
            if (!enemy.isStunned)
            {
                HeathManager.TakeDamage(1);
                PlayerController.playHurtsound();
                if (HeathManager.health <= 0)
                {
                    gameOver.GameOver();
                }
                else
                {
                    if (gameManager.GetCointCount() > 0)
                    {
                        gameManager.DropCoins(collision.transform.position);
                        gameManager.addScore(-3);
                        Debug.Log("Hit enemy");
                    }
                    StartCoroutine(GetHurt());
                }
                //if (gameManager.GetCointCoutn() > 0)
                //{
                //    gameManager.DropCoins(collision.transform.position);
                //    gameManager.addScore(-3);
                //    Debug.Log("Hit enemy");
                //}
                //else
                //    gameOver.GameOver();
            }
            //gameManager.addScore(-1);
            //StartCoroutine(enemy.Die());
        }
        if (collision.collider.CompareTag("EnemyHead"))
        {
            var enemy = collision.collider.GetComponentInParent<Enemy>();
            if(enemy.isStunned || player.isSpecialAttack)
            {
                enemy.isDead = true;
            }
            else
            {
                enemy.isStunned = true;
            }
        }
    }

    IEnumerator GetHurt()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        GetComponent<Animator>().SetLayerWeight(1, 1);
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().SetLayerWeight(1, 0);
        Physics2D.IgnoreLayerCollision(8, 9,false);

    }
}
