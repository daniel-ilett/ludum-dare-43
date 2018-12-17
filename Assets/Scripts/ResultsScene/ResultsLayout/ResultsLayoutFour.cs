using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsLayoutFour : ResultsLayout
{
	// Simply place large holders for all 4 players.
	protected override void Layout()
	{
		for(int i = 0; i < playerHolders.Count; ++i)
		{
			var newPlayer = Instantiate(resultsPlayerLarge, playerHolders[i]);
			resultsPlayers.Add(newPlayer);
		}
	}
}
