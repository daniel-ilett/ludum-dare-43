﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwordEntity : MonoBehaviour
{
	private bool settled = false;

	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;

	private void Awake()
	{
		// Set component properties.
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
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
					// Stick into level geometry.
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
		return renderer.sprite;
	}

	// Set child gameObject's sword sprite.
	public void SetSprite(Sprite sprite)
	{
		renderer.sprite = sprite;
	}

	// Give the sword a large velocity forwards and set gravity in 0.5s.
	public void Throw()
	{
		rigidbody.gravityScale = 0.0f;

		rigidbody.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);

		StartCoroutine(SetGravityActive());
	}

	// Move the sword forward slowly into the scene geometry.
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

	// Destroy the sword and remove it from the game.
	public void DestroySword()
	{
		Destroy(gameObject);
	}
}