using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] public int maxJump = 2;
    private bool isGrounded;
    private int JumpCount = 0;
    private Rigidbody2D rb;
    private int direction = 1;
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    //movement function
    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        if (x * direction < 0)
        {
            direction *= -1;
            transform.Rotate(0, 180, 0);
        }
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        float y = Input.GetAxis("Vertical");
        if ((Input.GetButtonDown("Jump") || y > 0) && JumpCount++ < maxJump-1)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if(isGrounded)
        {
            JumpCount = 0;
        }
    }
}
