using Unity.VisualScripting;
using UnityEngine;

public class DestructibleScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && player.isDashing)
            {
                Destroy(gameObject);
            }
        }
    }
}
