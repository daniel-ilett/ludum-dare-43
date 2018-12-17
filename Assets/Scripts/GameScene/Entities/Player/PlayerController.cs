using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer handSprite;

	[SerializeField]
	private Transform heldSwordsRoot;

	// Set the PlayerMovement connected input component when ours is set.
	private ConnectedInput _connectedInput;
	public ConnectedInput ConnectedInput
	{
		private get
		{
			return _connectedInput;
		}
		set
		{
			_connectedInput = value;
			movement.connectedInput = value;
		}
	}

	private bool isAlive = true;
	private bool isVulnerable = true;

	private SwordEntity justThrownSword;

	private List<SpriteRenderer> heldSwords;

	private Vector3 respawnLocation;

	// Component caching.
	private PlayerMovement movement;
	private new Rigidbody2D rigidbody;
	private Animator animator;

	public event EventHandler<SwordCountChangedEventArgs> SwordCountChanged;

	private void Awake()
	{
		movement = GetComponent<PlayerMovement>();
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		heldSwords = new List<SpriteRenderer>();

		// Remember spawn location for respawning.
		respawnLocation = transform.position;
	}

	// Game loop - called once per frame.
	private void Update()
	{
		if (isAlive)
		{
			movement.Move();

			if (ConnectedInput.PressedSwing())
			{
				SwingSword();
			}
			else if (ConnectedInput.PressedThrow())
			{
				ThrowSword();
			}
		}
	}

	// Physics update step, called every 1/60th of a second.
	private void FixedUpdate()
	{
		movement.DoFixedUpdate();
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
			var throwDir = ConnectedInput.GetMoveDir();

			if(throwDir.magnitude < 0.1f)
			{
				throwDir = new Vector3(movement.IsFacingRight() ? 1.0f : -1.0f, 0.0f, 0.0f);
			}

			newSword.transform.up = throwDir;

			newSword.Throw(ConnectedInput.GetPlayerID());

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
		SetIsAlive(false);

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
		
		SetIsAlive(true);
	}

	private void SetIsAlive(bool isAlive)
	{
		if(this.isAlive == isAlive)
		{
			return;
		}

		this.isAlive = isAlive;

		if (isAlive)
		{
			transform.position = respawnLocation;
			rigidbody.bodyType = RigidbodyType2D.Dynamic;
		}
		else
		{
			transform.position = new Vector3(100.0f, 0.0f, 0.0f);
			rigidbody.bodyType = RigidbodyType2D.Static;
			rigidbody.velocity = Vector3.zero;
		}
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
