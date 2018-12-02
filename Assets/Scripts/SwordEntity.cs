using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwordEntity : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private bool settled = false;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		// Set component properties.
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch(other.tag)
		{
			case "Player":
				{
					// Harm the player that was hit.
					PlayerController player = other.gameObject.GetComponent<PlayerController>();

					if (player != null)
					{
						if(settled)
						{
							player.PickupSword(this);
							Destroy(gameObject);
						}
						else
						{
							player.GetHitBySword(this);
						}
					}
				}
				break;
			case "Sword":
				// Do not collide with swords.
				return;
			default:
				{
					StartCoroutine(StickIntoGeometry());
				}
				break;
		}
	}

	private void Update()
	{
		if(!settled)
		{
			transform.up = rigidbody.velocity;
		}
	}

	// Wait for 0.5s and set gravity to be active.
	private IEnumerator SetGravityActive()
	{
		yield return new WaitForSeconds(0.5f);

		rigidbody.gravityScale = 1.0f;
	}

	// Get child object's sword sprite.
	public Sprite GetSprite()
	{
		return spriteRenderer.sprite;
	}

	// Set child gameObject's sword sprite.
	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	// Give the sword a large velocity forwards and set gravity in 0.5s.
	public void Throw()
	{
		rigidbody.gravityScale = 0.0f;

		rigidbody.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);

		StartCoroutine(SetGravityActive());
	}

	private IEnumerator StickIntoGeometry()
	{
		settled = true;

		Vector3 direction = rigidbody.velocity.normalized;
		rigidbody.bodyType = RigidbodyType2D.Static;

		for(float t = 0.0f; t < 0.25f; t += Time.deltaTime)
		{
			transform.position += direction * Time.deltaTime;
			yield return null;
		}
	}
}
