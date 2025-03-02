using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
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
            //var enemy = collision.collider.GetComponent<Enemy>();
            var gameOver = FindAnyObjectByType<GameOverScript>();
            gameOver.GameOver();
            //gameManager.addScore(-1);
            //StartCoroutine(enemy.Die());
        }
        if (collision.collider.CompareTag("EnemyHead"))
        {
            var enemy = collision.collider.GetComponentInParent<Enemy>();
            if(enemy.isStunned)
            {
                enemy.isDead = true;
            }
            else
            {
                enemy.isStunned = true;
            }
        }
    }

    private void dropCoin()
    {

    }
}
