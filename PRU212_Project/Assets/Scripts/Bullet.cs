using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Material blast;
    [SerializeField] public float speed = 10f;
    private Vector3 direction; // Direction to the target
    private float angle; // Angle to rotate the bullet

    public void SetTarget(Vector3 target, Vector3 firePoint)
    {

        direction = (target - firePoint).normalized;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation once
        transform.rotation = Quaternion.Euler(0, 0, angle);
        speed *= firePoint.x > target.x ? 1 : -1;
    }

    void Update()
    {
        // Move the bullet in the pre-calculated direction
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}