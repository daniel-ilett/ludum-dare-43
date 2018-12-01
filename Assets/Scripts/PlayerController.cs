using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Vector2 targetVelocity;
	private float moveSpeed = startMoveSpeed;

	private const float startMoveSpeed = 5.0f;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	// Game loop - called once per frame.
	private void Update()
	{
		Move();

		if (Input.GetButtonDown("Fire1"))
			SwingSword();
		else if (Input.GetButtonDown("Fire2"))
			ThrowSword();
	}

	// Move the player.
	private void Move()
	{
		targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 
			rigidbody.velocity.y);

		if (Input.GetButton("Jump"))
			targetVelocity = new Vector2(targetVelocity.x, moveSpeed);
	}

	// Every physics frame, set the velocity to target velocity.
	private void FixedUpdate()
	{
		rigidbody.velocity = targetVelocity;
	}

	// The player swings their sword around them to attack.
	private void SwingSword()
	{

	}

	// The player throws a sword in the direction they are moving.
	private void ThrowSword()
	{

	}

	// Recalculate the player run speed when a sword is thrown.
	private void RecalculateSpeed()
	{

	}
}
