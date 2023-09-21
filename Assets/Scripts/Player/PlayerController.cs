using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float groundedThresholdTime = 0.8f;
    [Range(0, 0.3f)][SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private bool airControl = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem walkEffect;

    private float m_HorizontalInput = 0f;
    public bool m_IsJumping = false, m_JumpInput = false;
    private float m_StartJumpingTime = -1000f;
    private float m_LastGroundedTime = -1000f;
    private bool m_FacingRight = true;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GetComponent<PlayerStats>().IsAlive() == false)
            return;

        // Retreive all inputs.

        m_HorizontalInput = Input.GetAxisRaw("Horizontal") * runSpeed;
        m_JumpInput = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        if (GetComponent<PlayerStats>().IsAlive() == false)
            return;

        bool isMoving = IsMoving();

        // Walk.

        if (IsGrounded(false) || airControl)
        {
            // Move the player in the correct direction.
            Vector3 targetVelocity = new Vector2(m_HorizontalInput * Time.fixedDeltaTime * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);

            // Flip the player if needed.
            if (m_HorizontalInput > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (m_HorizontalInput < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        // Jump.

        if (m_JumpInput == true)
        {
            if (IsGrounded(true) == true && m_IsJumping == false)
            {
                // Start impulse.
                m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                m_IsJumping = true;
                m_StartJumpingTime = Time.time;
            }
        }

        if(m_IsJumping == true)
        {
            if (IsGrounded(false) == true && Time.time - m_StartJumpingTime > 0.1)
            {
                m_IsJumping = false;
            }

            if(m_JumpInput == false)
            {
                m_IsJumping = false;
            }
        }

        // Animations and effects.

        animator.SetBool("isMoving", IsMoving());
        animator.SetBool("isGrounded", IsGrounded(false));

        if (isMoving == false || IsGrounded(false) == false)
        {
            var emissionModule = walkEffect.emission;
            emissionModule.rateOverTime = 0;
        }
        else if (isMoving == true && IsGrounded(false) == true)
        {
            var emissionModule = walkEffect.emission;
            emissionModule.rateOverTime = 20;
        }
    }

    private bool IsGrounded(bool threshold = false)
    {
        if(threshold == true)
        {
            if (Time.time - m_LastGroundedTime <= groundedThresholdTime)
                return true;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckTransform.position, 0.2f, groundLayer);
        if(colliders.Length > 0)
        {
            m_LastGroundedTime = Time.time;
            return true;
        }

        return false;
    }

    private bool IsMoving()
    {
        return Mathf.Abs(m_HorizontalInput) > 0.001f;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
