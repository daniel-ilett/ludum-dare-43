using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerGroundCheck : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerMask;

	public event EventHandler<EventArgs> HitGround;

	private new BoxCollider2D collider2D;

	private void Awake()
	{
		collider2D = GetComponent<BoxCollider2D>();
	}

	// Propogate the event to whoever wants to listen.
	private void OnTriggerEnter2D(Collider2D collision)
	{
		EventHandler<EventArgs> handler = HitGround;

		if(handler != null)
		{
			handler(this, new EventArgs());
		}
	}

	// Return whether the player is colliding.
	public bool IsColliding()
	{
		var col = Physics2D.OverlapArea(collider2D.bounds.min, collider2D.bounds.max, layerMask);

		return (col != null);
	}
}
