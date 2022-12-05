using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	public float runSpeed = 55f;

	public float horizontalMove = 0f;
	public bool jump = false;
	bool crouch = false;
	public bool isBouncy;
	public Collider2D normCollider;
	public Collider2D bounceCollider;
	public SpriteRenderer SR;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
            if (controller.m_Grounded == true)
			{
                controller.Jump.Play();
                controller.anim.SetTrigger("Jump");
            }
		}

        if (controller.m_Grounded == false && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Mouse1)))
        {
            isBouncy = true;
            controller.Spin.Play();
            Debug.Log("Spinning");
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            isBouncy = false;
			controller.Spin.Stop();
            Debug.Log("Spinning Over");
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (isBouncy == true || (controller.m_Grounded == false))
		{
			controller.Land.Play();
		}
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
        if (isBouncy == true)
		{
			normCollider.enabled = false;
			bounceCollider.enabled = true;
            //SR.color = Color.blue;
			controller.bounceParticle.Play();
        }
		else
		{
            normCollider.enabled = true;
            bounceCollider.enabled = false;
            //SR.color = Color.white;
            controller.bounceParticle.Stop();
        }
	}
}
