using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsLayoutEight : ResultsLayout
{
	[SerializeField]
	private ResultsPlayer resultsPlayerSmall;

	// Place large holders for the first three players. If there are fewer than
	// six players, also place large for 4 and 5. Else, place small holders
	// for players 4 to 8.
	protected override void Layout()
	{
		var playerCount = PointTracker.instance.GetPointSwords().Count;

		for(int i = 0; i < 3; ++i)
		{
			var newPlayer = Instantiate(resultsPlayerLarge, playerHolders[i]);
			resultsPlayers.Add(newPlayer);
		}

		if(playerCount > 5)
		{
			for(int i = 3; i < playerHolders.Count; ++i)
			{
				var newPlayer = Instantiate(resultsPlayerSmall, playerHolders[i]);
				resultsPlayers.Add(newPlayer);
			}
		}
		else
		{
			for(int i = 3; i < 5; ++i)
			{
				var newPlayer = Instantiate(resultsPlayerLarge, playerHolders[i]);
				resultsPlayers.Add(newPlayer);
			}
		}
	}
}
