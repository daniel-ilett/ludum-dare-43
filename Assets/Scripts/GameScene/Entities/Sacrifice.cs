using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour
{
	private Dictionary<int, int> playerPoints;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();

		// Set up the point totals counter.
		playerPoints = new Dictionary<int, int>();

		for(int i = 0; i < Connections.connectedInputs.Count; ++i)
		{
			playerPoints.Add(i + 1, 0);
		}
	}

	// Get hit by a sword and record the points for the player that hit.
	public void GetHitBySword(int playerID)
	{
		if(playerID > 0)
		{
			++playerPoints[playerID];
		}
	}

	// Add a force to the Rigidbody component of the sacrifice.
	public void AddForce(Vector3 force)
	{
		rigidbody.AddForce(force.normalized * 50.0f, ForceMode2D.Impulse);
	}
}
