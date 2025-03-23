using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Material blast;
    [SerializeField] AudioClip start;
    [SerializeField] public float speed = 10f;
    [SerializeField] public float ExistLimit = 4f;
    public int _Damage;
    private AudioSource audio;
    private Vector3 direction; // Direction to the target
    private float angle; // Angle to rotate the bullet
    private float existTime;

    public void SetTarget(Vector3 target, Vector3 firePoint, int Damage)
    {
        _Damage = Damage;
        audio = GetComponent<AudioSource>();
        direction = (target - firePoint).normalized;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation once
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = firePoint;
        audio.PlayOneShot(start);
        //speed *= firePoint.x > target.x ? 1 : -1;
    }

    void Update()
    {
        // Move the bullet in the pre-calculated direction
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        existTime += Time.deltaTime;
        if(existTime >= ExistLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
            Destroy(gameObject,0.2f);
    }
}