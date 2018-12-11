using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker : MonoBehaviour
{
	private Dictionary<int, List<Sprite>> playerPoints;

	public static PointTracker instance = null;

	public void Awake()
	{
		// Set self as singleton, overriding any others.
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}

		instance = this;
		DontDestroyOnLoad(gameObject);

		// Instantiate the list of swords thrown by each player.
		playerPoints = new Dictionary<int, List<Sprite>>();

		foreach (var connectedInput in Connections.connectedInputs)
		{
			playerPoints.Add(connectedInput.GetPlayerID(), new List<Sprite>());
		}
	}

	// Add a score by a player with a specific sword type.
	public void AddPoint(int playerID, Sprite sword)
	{
		playerPoints[playerID].Add(sword);
	}

	// Return whether one player has more points than all others.
	public bool IsAWinner()
	{
		bool isHighest = false;
		int highestCount = -1;

		foreach(var list in playerPoints.Values)
		{
			if (list.Count > highestCount)
			{
				highestCount = list.Count;
				isHighest = true;
			}
			else if (list.Count == highestCount)
			{
				isHighest = false;
			}
		}

		return isHighest;
	}

	// Return to boot screen, so tracker is invalid.
	public void EndGame()
	{
		instance = null;
		Destroy(gameObject);
	}
}
