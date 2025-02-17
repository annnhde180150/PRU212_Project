using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] public Animator animator;

    [Header("WallCheck")]
    [SerializeField] public Transform wallCheckpos;  

    [SerializeField] public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    bool isTouchingWall;

    [Header("PlayerWallJump")]
    [SerializeField] private float airMovementSpeed = 2f;
    private bool IswallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingForce = new Vector2(8f,16f);

    [Header("PlayerMovementJump")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] public int maxJump = 2;
    private bool isGrounded;
    private int JumpCount = 0;
    private Rigidbody2D rb;
    [SerializeField] private int direction = 1;


    [Header("PlayerDash")]
    [SerializeField] private float dashPower = 100f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    private float dashCountdown = 0;
    private int dashCount = 0;
    private bool isDashing = false;

   
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
        if (!isDashing)
        {
            Move();
            Jump();           
            CheckGrounded();
            wallSlide();
            WallJump();
            StartCoroutine(Dash());
            
        }
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
        if (isGrounded)
        {            
            animator.SetFloat("speed", Mathf.Abs(x));
            rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
        }else if(!isGrounded && !isWallSliding && x != 0)
        {
            rb.AddForce(new Vector2(airMovementSpeed* x,0),ForceMode2D.Force);            
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -moveSpeed, moveSpeed), rb.linearVelocity.y);           
        }

       

        return;            
    }

    private void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Vertical")) && JumpCount++ < maxJump - 1 && !isWallSliding)
        {
            Debug.Log("Jump");
            animator.SetBool("IsJump",true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
        }       
    } 
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded)
        {
            animator.SetBool("IsJump", false);
            JumpCount = 0;
        }
    }

    private void wallSlide()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheckpos.position, 0.1f, wallLayer);

        if (!isGrounded && isTouchingWall && rb.linearVelocityY < 0)
        {
            animator.SetBool("IsWallAni", true);
            isWallSliding = true;                     
        }
        else
        {
            animator.SetBool("IsWallAni", false);
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    public void WallJump()
    {
        if (isWallSliding)
        {
            IswallJumping = false;
            JumpCount = 0;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && isWallSliding)
        {
            Debug.Log("WallJump");
            IswallJumping = true;
            animator.SetBool("IsJump", true);
            rb.linearVelocity = new Vector2(wallJumpingForce.x * -direction, wallJumpingForce.y);           
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
        private IEnumerator Dash()
    {
        dashCountdown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount <1 && dashCountdown >= dashCooldown)
        {
            dashCountdown = 0;
            dashCount++;
            isDashing = true;

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(dashPower*direction , 0);
            tr.emitting = true;
            animator.SetBool("IsRoll", true);
            animator.SetBool("IsJump", false);
            yield return new WaitForSeconds(dashTime);
            rb.gravityScale = originalGravity;
            animator.SetBool("IsJump", true);
            animator.SetBool("IsRoll", false);
            isDashing = false;
            tr.emitting = false;
        }
        if(isGrounded || isTouchingWall) dashCount = 0;
    } 
    private void StopWallJumping()
    {
        IswallJumping = false;
    }

    
}



   

