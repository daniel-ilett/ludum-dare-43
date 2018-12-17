using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResultsLayout : MonoBehaviour
{
	[SerializeField]
	protected List<Transform> playerHolders;

	[SerializeField]
	protected ResultsPlayer resultsPlayerLarge;

	protected List<ResultsPlayer> resultsPlayers;

	// Programmatically place all elements for laying out 
	private void Awake()
	{
		resultsPlayers = new List<ResultsPlayer>();
		Layout();

		// Deactivate all player layout components.
		foreach(var resultsPlayer in resultsPlayers)
		{
			resultsPlayer.SetActive(false);
		}
	}

	protected abstract void Layout();

	// Set the playerID and points total of each layout area.
	public void SetPlayerStats(int playerID, int points)
	{
		if(resultsPlayers.Count >= playerID)
		{
			resultsPlayers[playerID - 1].SetStats(playerID, points);
		}
	}
}
