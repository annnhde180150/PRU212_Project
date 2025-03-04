using UnityEngine;

public class SlimeBlockScript : MonoBehaviour
{
    public float bounceForce = 5f; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 collisionNormal = collision.GetContact(0).normal;
            Vector2 oppositeDirection = -collisionNormal;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(oppositeDirection * bounceForce, ForceMode2D.Impulse);
            }

           
        }
    }
}
