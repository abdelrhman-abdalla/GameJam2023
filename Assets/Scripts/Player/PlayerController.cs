using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ParticleSystemJobs;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 0.3f)][SerializeField] private float m_MovementSmoothing = 0.05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem walkEffect;

    private float hInput = 0f;
    private bool jump = false;
    private bool m_Grounded = false;
    private bool m_FacingRight = true;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

        Move(hInput * Time.fixedDeltaTime, jump);
        jump = false;
    }


    public void Move(float move, bool jump)
    {
        // Movements
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // Jump
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        // Moving smoke effect.
        if (move == 0.0f || m_Grounded == false)
        {
            var emissionModule = walkEffect.emission;
            emissionModule.rateOverTime = 0;
        }
        else if (move != 0.0f && m_Grounded == true)
        {
            var emissionModule = walkEffect.emission;
            emissionModule.rateOverTime = 20;
        }

        // Walking animation.
        animator.SetBool("isMoving", move != 0.0f);

        // Jumping animation.
        animator.SetBool("isGrounded", m_Grounded);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
