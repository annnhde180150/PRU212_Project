using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject landingAnimation;
    [SerializeField] public Transform animationPoint;
    private Animator dustAnimator;
    private bool islanding;

    [Header("WallCheck")]
    [SerializeField] public Transform wallCheckpos;
    [SerializeField] public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    bool isTouchingWall;

    [Header("PlayerWallJump")]
    [SerializeField] private float airMovementSpeed = 2f;
    //private bool IswallJumping;
    //private float wallJumpingDirection;
    //private float wallJumpingTime;
    //private float wallJumpingCounter;
    //private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingForce = new Vector2(8f, 16f);

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
    public bool isDashing = false;

    [Header("PlayerSpecialAttack")]
    [SerializeField] private float specialAttackPower = 10f;
    [SerializeField] private float specialAttackTime = 1f;
    [SerializeField] private float specialAttackCooldown = 1f;
    private float specialAttackCountdown = 0;
    private int specialAttackCount = 0;
    public bool isSpecialAttack = false;

    [Header("PlayerDuck")]
    public bool isDuck = false;
    private float duckHoldTime = 0f; 
    private bool isHoldingDuck = false;
    [SerializeField]  private float DuckForce = 10f;
    private bool isDuckJump = false;

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
            StartCoroutine(SpecialAttack());
            StartCoroutine(Duck());
        }
    }

    //Create dust when player jump, land
    private void CreateDust(string triggerName)
    {
        GameObject playerDust = Instantiate(landingAnimation, animationPoint.position, Quaternion.identity);
        dustAnimator = playerDust.GetComponent<Animator>();
        if (dustAnimator != null)
        {
            dustAnimator.SetTrigger(triggerName);
        }
        Destroy(playerDust, 0.5f);
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
        }
        else if (!isGrounded && !isWallSliding && x != 0)
        {
            rb.AddForce(new Vector2(airMovementSpeed * x, 0), ForceMode2D.Force);
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -moveSpeed, moveSpeed), rb.linearVelocity.y);
        }

        if (isDuck)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
            //animator.SetFloat("speed", 0); 
            return;
        }

        return;
    }

    private void Jump()
    {
        //animator.SetBool("IsJump", true);
        if ((Input.GetButtonDown("Jump")) && JumpCount < maxJump - 1 && !isWallSliding)
        {
            CreateDust("JumpDustTrigger");
            JumpCount = Mathf.Clamp(JumpCount + 1, 0, maxJump);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

    }
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded)
        {
            animator.SetBool("IsSpecialAttackFinish", true);
            animator.SetBool("IsJumpFinish", true);
            JumpCount = 0;

        }
        else
        {
            animator.SetBool("IsJumpFinish", false);
            animator.SetTrigger("IsJumpTrigger");
            islanding = true;
        }

        if (isSpecialAttack && isGrounded)
        {
            CreateDust("StompDustTrigger");
            isSpecialAttack = false;
        }

        if (islanding && isGrounded && !isSpecialAttack)
        {
            CreateDust("LandDustTrigger");
            islanding = false;
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
            JumpCount = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    public void WallJump()
    {
        if (Input.GetButtonDown("Jump") && isWallSliding)
        {
            Debug.Log("WallJump");
            animator.SetTrigger("IsJumpTrigger");
            rb.linearVelocity = new Vector2(wallJumpingForce.x * -direction, wallJumpingForce.y);
        }
    }
    private IEnumerator Dash()
    {
        dashCountdown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount < 1 && dashCountdown >= dashCooldown)
        {
            dashCountdown = 0;
            dashCount++;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            if (((Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.RightArrow))) || ((Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.LeftArrow))))
            {
                float dashAngle = 45f * Mathf.Deg2Rad;
                rb.linearVelocity = new Vector2(dashPower * direction, dashPower * Mathf.Sin(dashAngle));

            }
            else
            {
                rb.linearVelocity = new Vector2(dashPower * direction, 0);
            }
            tr.emitting = true;
            animator.SetBool("IsRollFinish", false);
            animator.SetTrigger("IsRollTrigger");
            yield return new WaitForSeconds(dashTime);
            rb.gravityScale = originalGravity;
            animator.SetBool("IsRollFinish", true);
            isDashing = false;
            tr.emitting = false;
        }
        if (isGrounded || isTouchingWall) dashCount = 0;
    }

    private IEnumerator SpecialAttack()
    {
        specialAttackCountdown += Time.deltaTime;
        //
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isGrounded && specialAttackCount < 1 && specialAttackCountdown >= specialAttackCooldown)
        {
            specialAttackCountdown = 0;
            specialAttackCount++;
            isSpecialAttack = true;
            animator.SetBool("IsSpecialAttackFinish", false);
            animator.SetTrigger("IsSpecialAttackTrigger");
            Debug.Log("isSpecialAttack " + isSpecialAttack);
            rb.linearVelocity = new Vector2(0, -specialAttackPower);
            yield return new WaitForSeconds(specialAttackTime);
            animator.SetBool("IsSpecialAttackFinish", true);
            isSpecialAttack = false;

        }
        if (isGrounded || isTouchingWall) specialAttackCount = 0;
    }

    private IEnumerator Duck()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!isHoldingDuck)
            {
                isHoldingDuck = true;
                duckHoldTime = 0f;
            }
            duckHoldTime += Time.deltaTime;
            isDuck = true;
            Debug.Log("isDuck " + isDuck);
            animator.SetBool("IsDuckFirst", true);
            animator.SetBool("IsDuckFirstFinish", false);
            if (duckHoldTime >= 1f)
            {
                animator.SetBool("IsDuckFirst", false);
                animator.SetBool("IsDuckSecond", true);
                isDuckJump = true;
            }
        }
        else
        {
            
            if (isHoldingDuck)
            {
                isDuck = false;
                isHoldingDuck = false;                
                animator.SetBool("IsDuckFirstFinish", true);
                animator.SetBool("IsDuckFirst", false);
            }           
            if (duckHoldTime >= 1f && isDuckJump) 
            {
                animator.SetBool("IsDuckFirst", false);
                animator.SetBool("IsDuckSecond", false);
                isDuckJump = false;
                rb.linearVelocity = new Vector2(0, DuckForce);
                yield return new WaitForSeconds(1f);
                isDuck = false;
                Debug.Log("isDuck " + isDuck);
            }
        }
    }
}



   

