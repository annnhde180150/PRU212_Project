using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Material blast;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (_spriteRenderer != null) _spriteRenderer.material = blast;
        Destroy(gameObject, 0.2f);
    }
}
