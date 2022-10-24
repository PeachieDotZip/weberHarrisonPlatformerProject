using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	public float runSpeed = 50f;

	float horizontalMove = 0f;
	bool jump = false;
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
		}

        if (controller.m_Grounded == false && Input.GetKeyDown(KeyCode.W))
        {
            isBouncy = true;
            Debug.Log("Spinning");
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            isBouncy = false;
            Debug.Log("Spinning Over");
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
            SR.color = Color.blue;
        }
		else
		{
            normCollider.enabled = true;
            bounceCollider.enabled = false;
            SR.color = Color.white;
        }
	}
}
