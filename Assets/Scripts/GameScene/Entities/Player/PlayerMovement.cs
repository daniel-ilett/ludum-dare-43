using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
	public ConnectedInput connectedInput { private get; set; }

	private const float maxMoveSpeed = 5.0f;
	private float moveSpeed = maxMoveSpeed;
	private Vector2 targetVelocity;

	private bool facingRight = true;

	private JumpState jumpState = JumpState.AirJumped;

	// Component caching.
	private new Rigidbody2D rigidbody;
	private Animator animator;
	private PlayerGroundCheck groundCheck;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		groundCheck = GetComponentInChildren<PlayerGroundCheck>();
	}

	private void Start()
	{
		// Subscribe to the ground-checking event for reaching the ground.
		groundCheck.HitGround += HitGround;
	}

	// Move the player.
	public void Move()
	{
		targetVelocity = new Vector2(connectedInput.GetHorizontal() * moveSpeed,
			rigidbody.velocity.y);

		// Set the "walking" animation.
		animator.SetBool("IsWalking", targetVelocity.magnitude > 0.1f);

		if (connectedInput.PressedJump())
		{
			if (groundCheck.IsColliding())
			{
				// Prepare to jump next FixedUpdate().
				jumpState = JumpState.Jumping;
			}
			else if (jumpState == JumpState.Jumped)
			{
				jumpState = JumpState.AirJumping;
			}
		}

		// If the player turns around, flip their facing direction.
		if ((facingRight && connectedInput.GetHorizontal() < -0.1f) ||
			(!facingRight && connectedInput.GetHorizontal() > 0.1f))
		{
			FaceDirection(!facingRight);
		}
	}

	// Is the player facing right?
	public bool IsFacingRight()
	{
		return facingRight;
	}

	// Every physics frame, set the velocity to target velocity.
	// Crucially, this is called from the outside by PlayerController.
	public void DoFixedUpdate()
	{
		if (jumpState == JumpState.Jumping || jumpState == JumpState.AirJumping)
		{
			// Jump and set the JumpState accordingly.
			targetVelocity = new Vector2(targetVelocity.x, moveSpeed * 2.0f);
			jumpState = (jumpState == JumpState.Jumping) ? JumpState.Jumped : JumpState.AirJumped;
		}
	
		rigidbody.velocity = targetVelocity;
	}

	// Scale the player so they face a particular direction.
	private void FaceDirection(bool facingRight)
	{
		if (this.facingRight != facingRight)
		{
			transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, 1.0f);
			this.facingRight = facingRight;
		}
	}

	// Event received when the GroundCheck hits ground.
	private void HitGround(object sender, EventArgs e)
	{
		jumpState = JumpState.Grounded;
	}
}
