using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	ResultsLayout is the root transform for the 2-8 ResultsPlayer objects on the
 *	Results screen. It dynamically creates the required number of objects, then
 *	controls the assignment of data to each object.
 */
public class ResultsLayout : MonoBehaviour
{
	[SerializeField]
	protected ResultsPlayer resultsPlayer;

	protected List<ResultsPlayer> resultsPlayers;

	// Programmatically place all layout elements.
	public void LayoutResults(int playerCount)
	{
		resultsPlayers = new List<ResultsPlayer>();

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
