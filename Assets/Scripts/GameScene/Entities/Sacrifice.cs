using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour
{
	private Dictionary<int, int> playerPoints;

	private void Awake()
	{
		// Set up the point totals counter.
		playerPoints = new Dictionary<int, int>();

		for(int i = 0; i < ConnectionController.connectedInputs.Count; ++i)
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
}
