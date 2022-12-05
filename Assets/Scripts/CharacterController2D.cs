using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 800f;                           // Amount of force added when the player jumps.
	//[Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private bool ForcingRight = false;
	private bool ForcingLeft = false;
	public GameObject target;
	private PlayerMovement PM;
	public bool canChomp = true;
	public bool OpenMouth = false;
	public ParticleSystem dustParticle;
	public ParticleSystem dashParticle;
    public ParticleSystem bounceParticle;
    public ParticleSystem deadParticle;
    public CanvasScript CS;
	public Animator anim;
	public Animator MainCamera;
	private bool LandSFX_Ready;
	//Audio
	public AudioSource Jump;
    //public AudioSource Walk;
    public AudioSource Land;
    public AudioSource Chomp;
    public AudioSource Spin;
    public AudioSource Death;
	public AudioSource Step;


    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		PM = GetComponent<PlayerMovement>();
	}


	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
        }
		if (ForcingRight == true)
		{
			m_Rigidbody2D.AddForce(new Vector2(270f, 0f));
		}
        if (ForcingLeft == true)
        {
            m_Rigidbody2D.AddForce(new Vector2(-270f, 0f));
        }
        if (OpenMouth == true)
        {
            m_Rigidbody2D.AddForce((target.transform.position - transform.position).normalized * -10f);
        }
		if (m_Grounded)
		{
			LandSFX_Ready = false;
			if (LandSFX_Ready == true)
			{
                Land.Play();
            }
        }
        m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, 55);
    }

	void Update()
	{
		//Animation Stuff
		//anim.SetFloat("Speed", m_Rigidbody2D.velocity.magnitude);
		anim.SetBool("isGrounded", m_Grounded);
		anim.SetBool("Bouncy", PM.isBouncy);
		//anim.SetBool("Jump", PM.jump);
		if (PM.horizontalMove == 0f)
		{
			anim.SetBool("Moving", false);
        }
		else
		{
            anim.SetBool("Moving", true);
        }
	}
	//Jump-Animation Bug Fix
	public void CanLand_False()
	{
        anim.SetBool("CanLand", false);
    }
	public void CanLand_True()
	{
		anim.SetBool("CanLand", true);
		LandSFX_Ready = true;
	}
	//Step SFX void for Walk animation
	public void StepSFX()
	{
		Step.Play();
	}

	public void Move(float move, bool crouch, bool jump)
	{


		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

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
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			dustParticle.Play();
		}
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
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collision");
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Bouncer"))
		{
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.AddForce(new Vector2(0f, 1300f));
			anim.SetTrigger("Jump");
			Jump.Play();
		}
		if (collision.gameObject.CompareTag("BouncerRight"))
		{
			m_Rigidbody2D.velocity = Vector2.zero;
            m_MovementSmoothing = 0.25f;
            m_Rigidbody2D.AddForce(new Vector2(1600f, 1215f));
            StartCoroutine(ForceRight());
            anim.SetTrigger("Jump");
            Jump.Play();
        }
		if (collision.gameObject.CompareTag("BouncerLeft"))
		{
			m_Rigidbody2D.velocity = Vector2.zero;
            m_MovementSmoothing = 0.25f;
            m_Rigidbody2D.AddForce(new Vector2(-1600f, 1215f));
            StartCoroutine(ForceLeft());
            anim.SetTrigger("Jump");
            Jump.Play();
        }
		if (collision.gameObject.CompareTag("Hurt"))
		{
			StartCoroutine(Hurt());
            Death.Play();
            CS.UIanim.SetTrigger("Respawn");
        }
	}
    public IEnumerator LightChomp()
    {
		anim.SetTrigger("Chomp");
        MainCamera.SetTrigger("Chomp");
        Chomp.Play();
        canChomp = false;
        OpenMouth = true;
        Debug.Log("OPEN");
        m_Rigidbody2D.velocity = Vector2.zero;
		PM.runSpeed = 0f;
		m_AirControl = false;
		m_Rigidbody2D.gravityScale = 0f;
        m_MovementSmoothing = 0.05f;
        StopCoroutine(ForceLeft());
        StopCoroutine(ForceRight());
        yield return new WaitForSeconds(0.60f);
		dashParticle.Play();
        Debug.Log("CHOMP");
        m_Rigidbody2D.gravityScale = 6.3f;
        m_MovementSmoothing = 0.25f;
        OpenMouth = false;
        PM.runSpeed = 55f;
        m_Rigidbody2D.AddForce((target.transform.position - transform.position).normalized * 5000f);
        yield return new WaitForSeconds(0.3f);
		canChomp = true;
        dashParticle.Stop();
        yield return new WaitForSeconds(0.5f);
        m_MovementSmoothing = 0.05f;
        m_Rigidbody2D.gravityScale = 3f;
        m_AirControl = true;
    }

    public IEnumerator ForceRight()
	{
        //ForcingRight = true;
		m_MovementSmoothing = 0.24f;
        yield return new WaitForSeconds(1.35f);
		ForcingRight = false;
        if (m_MovementSmoothing == 0.24f)
        {
            m_MovementSmoothing = 0.05f;
        }
    }
    public IEnumerator ForceLeft()
    {
        //ForcingLeft = true;
        m_MovementSmoothing = 0.24f;
        yield return new WaitForSeconds(1.35f);
        ForcingLeft = false;
		if (m_MovementSmoothing == 0.24f)
		{
            m_MovementSmoothing = 0.05f;
        }
    }
    public IEnumerator PlayBounceParticle()
    {
		//Plays the bounce particle effect, and then stops it soon after
		bounceParticle.Play();
        yield return new WaitForSeconds(0.5f);
		bounceParticle.Stop();
    }
    public IEnumerator Hurt()
    {
		//Process of dying and respawning
        //PM.SR.color = Color.red;
		anim.SetTrigger("Death");
        deadParticle.Play();
        yield return new WaitForSeconds(1.65f);
        anim.ResetTrigger("Death");
        deadParticle.Stop();
    }
}
