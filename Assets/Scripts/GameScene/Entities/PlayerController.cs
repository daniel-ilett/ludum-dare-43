using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer handSprite;

	[SerializeField]
	private Transform heldSwordsRoot;

	[SerializeField]
	private PlayerGroundCheck groundCheck;

	public ConnectedInput connectedInput { private get; set; }

	private Vector2 targetVelocity;
	private float moveSpeed = startMoveSpeed;
	private bool jumped = false;

	private bool isAlive = true;

	private bool facingRight = true;
	private bool hasAirJumped = false;

	private SwordEntity justThrownSword;

	private List<SpriteRenderer> heldSwords;

	private const float startMoveSpeed = 5.0f;
	private Vector3 respawnLocation;

	private new Rigidbody2D rigidbody;
	private Animator animator;

	public event EventHandler<SwordCountChangedEventArgs> SwordCountChanged;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		heldSwords = new List<SpriteRenderer>();

		// Remember spawn location for respawning.
		respawnLocation = transform.position;

		// Subscribe to the ground-checking event for reaching the ground.
		groundCheck.HitGround += HitGround;
	}

	// Game loop - called once per frame.
	private void Update()
	{
		Move();

		if (connectedInput.PressedSwing())
		{
			SwingSword();
		}
		else if (connectedInput.PressedThrow())
		{
			ThrowSword();
		}
	}

	// Move the player.
	private void Move()
	{
		targetVelocity = new Vector2(connectedInput.GetHorizontal() * moveSpeed, 
			rigidbody.velocity.y);

		// Set the "walking" animation.
		animator.SetBool("IsWalking", targetVelocity.magnitude > 0.1f);

		if (connectedInput.PressedJump())
		{
			if(groundCheck.IsColliding())
			{
				jumped = true;
			}
			else if(!hasAirJumped)
			{
				hasAirJumped = true;
				jumped = true;
			}
		}

		// If the player turns around, flip their facing direction.
		if((facingRight && connectedInput.GetHorizontal() < -0.1f) ||
			(!facingRight && connectedInput.GetHorizontal() > 0.1f))
		{
			FaceDirection(!facingRight);
		}
	}

	// Scale the player so they face a particular direction.
	private void FaceDirection(bool facingRight)
	{
		if(this.facingRight != facingRight)
		{
			transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, 1.0f);
		}

		this.facingRight = facingRight;

		/*
		if(facingRight)
		{
			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
		else
		{
			transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
		}
		*/
	}

	// Every physics frame, set the velocity to target velocity.
	private void FixedUpdate()
	{
		if(isAlive)
		{
			if (jumped)
			{
				targetVelocity = new Vector2(targetVelocity.x, moveSpeed * 2.0f);
				jumped = false;
			}

			rigidbody.velocity = targetVelocity;
		}
	}

	// The player swings their sword around them to attack.
	private void SwingSword()
	{
		// Do a swing animation and attempt to hurt something.
	}
	
	// The player throws a sword in the direction they are moving.
	private void ThrowSword()
	{
		if(heldSwords.Count > 0 || handSprite.sprite != null)
		{
			// Play the throw animation.
			animator.SetTrigger("Throw");

			// Create sword entity and throw it.
			var newSword = SwordManager.instance.CreateSword(transform.position, Quaternion.identity);
			var throwDir = connectedInput.GetMoveDir();

			if(throwDir.magnitude < 0.1f)
			{
				throwDir = new Vector3(facingRight ? 1.0f : -1.0f, 0.0f, 0.0f);
			}

			newSword.transform.up = throwDir;

			newSword.Throw(connectedInput.GetPlayerID());

			justThrownSword = newSword;
			Invoke("ForgetSword", 0.25f);

			if (heldSwords.Count > 0)
			{
				var lastSword = heldSwords[heldSwords.Count - 1];
				newSword.SetSprite(lastSword.sprite);
				heldSwords.Remove(lastSword);
				Destroy(lastSword);
			}
			else
			{
				newSword.SetSprite(handSprite.sprite);
				handSprite.sprite = null;
			}

			OnSwordCountChanged(new SwordCountChangedEventArgs(heldSwords.Count));
		}
	}

	// The player can add a sword to their collection.
	public void PickupSword(SwordEntity sword)
	{
		// Blah blah blah.
		OnSwordCountChanged(new SwordCountChangedEventArgs(heldSwords.Count));

		// Create a sword sprite to put in the player's hand or on their back.
		if(handSprite.sprite == null)
		{
			handSprite.sprite = sword.GetSprite();
		}
		else
		{
			Quaternion holdRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f + UnityEngine.Random.Range(-30.0f, 15.0f));
			var newHeldSword = SwordManager.instance.CreateHeldSword(heldSwordsRoot, holdRotation);
			newHeldSword.sprite = sword.GetSprite();

			heldSwords.Add(newHeldSword);
		}
	}

	// Fire an event whenever the sword count changes.
	private void OnSwordCountChanged(SwordCountChangedEventArgs e)
	{
		EventHandler<SwordCountChangedEventArgs> handler = SwordCountChanged;
		
		if(handler != null)
		{
			handler(this, e);
		}
	}

	// Recalculate the player run speed when a sword is thrown.
	private void RecalculateSpeed()
	{

	}

	// Event received when the GroundCheck hits ground.
	private void HitGround(object sender, EventArgs e)
	{
		hasAirJumped = false;
	}

	// Forget you just threw a sword.
	private void ForgetSword()
	{
		justThrownSword = null;
	}

	// Apply damage to the player.
	public bool GetHitBySword(SwordEntity sword)
	{
		// Only get hit by swords we didn't just throw.
		if(sword != justThrownSword)
		{
			KillPlayer();
			return true;
		}
		else
		{
			return false;
		}
	}

	// Set the absolute position of the player along X.
	public void SetXPosition(Vector3 position)
	{
		var newPos = transform.position;
		newPos.x = position.x;
		transform.position = newPos;
	}

	// Instantly kill the player.
	public void KillPlayer()
	{
		isAlive = false;
		transform.position = new Vector3(100.0f, 0.0f, 0.0f);
		rigidbody.bodyType = RigidbodyType2D.Static;

		// Reset all sword-holding parameters.
		foreach(Transform sword in heldSwordsRoot)
		{
			Destroy(sword.gameObject);
		}

		heldSwords = new List<SpriteRenderer>();
		handSprite.sprite = null;
		justThrownSword = null;

		OnSwordCountChanged(new SwordCountChangedEventArgs(heldSwords.Count));
		StartCoroutine(Respawn());
	}

	// Wait a set time then respawn the player.
	private IEnumerator Respawn()
	{
		var wait = new WaitForSeconds(1.0f);

		for(int i = 0; i < 2; ++i)
		{
			yield return wait;
		}

		transform.position = respawnLocation;
		rigidbody.bodyType = RigidbodyType2D.Dynamic;
		rigidbody.velocity = Vector3.zero;

		isAlive = true;
	}
}

public class SwordCountChangedEventArgs : EventArgs
{
	public int swordCount { get; private set; }

	public SwordCountChangedEventArgs(int swordCount)
	{
		this.swordCount = swordCount;
	}
}
