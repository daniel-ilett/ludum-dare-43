using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour
{
	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	// Give points to player that threw the sword and add a force.
	public void GetHitBySword(int playerID, Sprite sword, Vector3 force)
	{
		if(playerID > 0)
		{
			PointTracker.instance.AddPoint(playerID, sword);
		}

		var impulse = (playerID > 0) ? 5.0f : 1.0f;

		rigidbody.AddForce(force.normalized * impulse, ForceMode2D.Impulse);
	}
}
