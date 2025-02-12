using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("wallCheck")]
    [SerializeField] public Transform wallCheckpos;  
    [SerializeField] public LayerMask wallLayer;

    [Header("wallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    bool isTouchingWall;

    [Header("PlayerWallJump")]
    [SerializeField] bool canMove;
    bool wallJumping;

    [Header("PlayerMovementJump")]
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
        canMove = true;
        wallJumping = false;
    }

    // Update is called once per frame
    void Update()
    {            
        Move();
        Jump();
        wallSlide();       
    }

    //movement function
    private void Move()
    {
        if (canMove)
        {
            float x = Input.GetAxis("Horizontal");
            if (x * direction < 0)
            {
                direction *= -1;
                transform.Rotate(0, 180, 0);
            }
            rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
            return;
        }               
    }

    private void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Vertical")) && JumpCount++ < maxJump-1)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        
        
        if(isGrounded)
        {
            wallJumping = false;
            JumpCount = 0;           
        }

        //Wall Jump
        if (wallJumping && Input.GetButtonDown("Jump"))
        {
            WallJump();
            JumpCount = 0;
            wallJumping = false;
        }
    }
    private void wallSlide()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheckpos.position, 0.1f, wallLayer);
        if (!isGrounded & isTouchingWall)
        {
            wallJumping = true;
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
        {            
            isWallSliding = false;
        }
            
            
    }

    public void WallJump()
    {        
        //StartCoroutine(DissableMovement(0.1f));  
        //Vector2 wallDirection = isTouchingWall ? Vector2.right : Vector2.left;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);       
    }
    IEnumerator DissableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}



   

