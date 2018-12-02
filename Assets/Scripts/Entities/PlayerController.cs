using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer handSprite;

	[SerializeField]
	private Transform heldSwordsRoot;

	private Vector2 targetVelocity;
	private float moveSpeed = startMoveSpeed;

	private SwordEntity justThrownSword;

	private List<SpriteRenderer> heldSwords;

	private const float startMoveSpeed = 5.0f;

	private new Rigidbody2D rigidbody;

	public event EventHandler<SwordCountChangedEventArgs> SwordCountChanged;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		heldSwords = new List<SpriteRenderer>();
	}

	// Game loop - called once per frame.
	private void Update()
	{
		Move();

		if (Input.GetButtonDown("Fire1"))
		{
			SwingSword();
		}
		else if (Input.GetButtonDown("Fire2"))
		{
			ThrowSword();
		}
	}

	// Move the player.
	private void Move()
	{
		targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 
			rigidbody.velocity.y);

		if (Input.GetButtonDown("Jump"))
		{
			targetVelocity = new Vector2(targetVelocity.x, moveSpeed * 2.0f);
		}
	}

	// Every physics frame, set the velocity to target velocity.
	private void FixedUpdate()
	{
		rigidbody.velocity = targetVelocity;
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
			// Create sword entity and throw it.
			var newSword = SwordManager.instance.CreateSword(transform.position, Quaternion.identity);
			newSword.transform.up = (transform.position + (Vector3)targetVelocity);
			newSword.Throw();

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

	// Forget you just threw a sword.
	private void ForgetSword()
	{
		justThrownSword = null;
	}

	// Apply damage to the player.
	public void GetHitBySword(SwordEntity sword)
	{
		// Only get hit by swords we didn't just throw.
		if(sword != justThrownSword)
		{

		}
	}

	// Instantly kill the player.
	public void KillPlayer()
	{
		Debug.Log("Killed player");
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
