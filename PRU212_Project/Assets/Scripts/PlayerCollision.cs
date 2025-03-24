using System.Collections;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController player;

    public AudioClip dropSound;
    public AudioClip collectSound;
    private AudioSource audioSource;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = FindAnyObjectByType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var gameOver = FindAnyObjectByType<GameOverScript>();
        if (collision.CompareTag("Coin"))
        {
            // Play coin collect sound
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            Destroy(collision.gameObject);
            gameManager.addScore(1);
        }

        if (collision.CompareTag("Boss"))
        {
            //- boss damage
            var enemy = collision.GetComponentInParent<Bullet>();
            var enemy2 = collision.GetComponentInParent<BossHand>();
            var damage = enemy == null? enemy2.Damage : enemy._Damage;
            HeathManager.TakeDamage((int)damage);
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
                    if (dropSound != null)
                    {
                        AudioSource.PlayClipAtPoint(dropSound, transform.position);
                    }
                    Debug.Log("Hit enemy");
                }
                StartCoroutine(GetHurt());
            }
        }

        //if (collision.CompareTag("BossHand"))
        //{
        //    Destroy(collision.gameObject);
        //    player.isSpecialAttack = true;
        //}
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
        var gameOver = FindAnyObjectByType<GameOverScript>();
        if (collision.collider.CompareTag("Enemy"))
        {
            var enemy = collision.collider.GetComponent<Enemy>();
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
                        if (dropSound != null)
                        {
                            AudioSource.PlayClipAtPoint(dropSound, transform.position);
                        }
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

        //boss
        if (collision.collider.CompareTag("Boss"))
        {
            var enemy = collision.collider.GetComponent<BossEnemy>();
            var baseStat = collision.collider.GetComponent<Enemy>();
            if (player.isSpecialAttack)
            {
                //enemy.isDead = true;
                //gameManager.addScore(1);
            }
            else
            if (!enemy.isStunned && !enemy.isDead)
            {
                HeathManager.TakeDamage((int)baseStat.damage);
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
                        if (dropSound != null)
                        {
                            AudioSource.PlayClipAtPoint(dropSound, transform.position);
                        }
                        Debug.Log("Hit enemy");
                    }
                    StartCoroutine(GetHurt());
                }
            }
        }
        if (collision.collider.CompareTag("BossHead"))
        {
            var enemy = collision.collider.GetComponentInParent<Enemy>();
            var baseStat = collision.collider.GetComponent<Enemy>();
            if (enemy.isStunned && player.isSpecialAttack)
            {
                enemy.health -= 2;
            }else if (enemy.isStunned)
            {
                enemy.health -= 1;
            }
            else
            {
                HeathManager.TakeDamage((int)baseStat.damage);
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
                        if (dropSound != null)
                        {
                            AudioSource.PlayClipAtPoint(dropSound, transform.position);
                        }
                    }
                    StartCoroutine(GetHurt());
                }
            }
        }
    }

    IEnumerator GetHurt()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        GetComponent<Animator>().SetLayerWeight(1, 1);
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().SetLayerWeight(1, 0);
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
