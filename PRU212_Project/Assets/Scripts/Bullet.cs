using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Material blast;
    [SerializeField] public float speed = 10f;
    public Vector3 _target;
    private Vector3 direction; // Direction to the target
    private float angle; // Angle to rotate the bullet

    public void SetTarget(Vector2 target)
    {
        Debug.Log("Bullet Position: " + transform.position);
        Debug.Log("Target Position: " + target);
        _target = target;

        direction = (_target - transform.position).normalized;
        Debug.Log("Direction: " + direction);

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("Angle: " + angle);

        // Set the rotation once
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        // Move the bullet in the pre-calculated direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 1f);
    }
}