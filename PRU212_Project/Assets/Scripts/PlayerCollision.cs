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
            Debug.Log("Hit coin");
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
            Debug.Log("Hit enemy");
            var enemy = collision.collider.GetComponentInParent<Enemy>() ?? collision.collider.GetComponent<Enemy>();
            enemy.isStunned = true;
            //gameManager.addScore(-1);
            //enemy.Die();
        }
    }

    private void dropCoin()
    {

    }
}
