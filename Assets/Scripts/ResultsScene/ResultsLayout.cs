using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsLayout : MonoBehaviour
{
	[SerializeField]
	protected ResultsPlayer resultsPlayer;

	protected List<ResultsPlayer> resultsPlayers;

	// Programmatically place all layout elements.
	private void Awake()
	{
		resultsPlayers = new List<ResultsPlayer>();

		var playerCount = PointTracker.instance.GetPointSwords().Count;

		for(int i = 0; i < playerCount; ++i)
		{
			var newPlayer = Instantiate(resultsPlayer, transform);
			resultsPlayers.Add(newPlayer);
		}
	}

	// Set the playerID and points total of each layout area.
	public void SetPlayerStats(int resultID, int rank, int playerID, int points)
	{
		if(resultsPlayers.Count > resultID)
		{
			resultsPlayers[resultID].SetStats(rank, playerID, points);
		}
	}
}
