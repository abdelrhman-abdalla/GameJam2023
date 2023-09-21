using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float jumpForce = 400f;
    [Range(0, 0.3f)][SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private bool airControl = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem walkEffect;

    private float m_HorizontalInput = 0f;
    private bool m_IsJumping = false;
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

        if (Input.GetButtonDown("Jump"))
        {
            m_IsJumping = true;
        }
    }

    private void FixedUpdate()
    {
        if (GetComponent<PlayerStats>().IsAlive() == false)
            return;

        bool isGrounded = IsGrounded();
        bool isMoving = IsMoving();

        // Walk.

        if (isGrounded || airControl)
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

        if (isGrounded && m_IsJumping)
        {
            // Add a vertical force to the player.
            m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }

        m_IsJumping = false;

        // Animations and effects.

        animator.SetBool("isMoving", IsMoving());
        animator.SetBool("isGrounded", isGrounded);

        if (isMoving == false || isGrounded == false)
        {
            var emissionModule = walkEffect.emission;
            emissionModule.rateOverTime = 0;
        }
        else if (isMoving == true && isGrounded == true)
        {
            var emissionModule = walkEffect.emission;
            emissionModule.rateOverTime = 20;
        }
    }

    private bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckTransform.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
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
