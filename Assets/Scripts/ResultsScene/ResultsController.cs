using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsController : MonoBehaviour
{
	[SerializeField]
	private Image backgroundColour;

	[SerializeField]
	private ResultsLayout playerLayout;

	private void Start()
	{
		StartCoroutine(CountScores());
	}

	// Count and animate the scores for each player.
	private IEnumerator CountScores()
	{
		var swordsUsed = PointTracker.instance.GetPointSwords();

		int resultID = 0;

		int rank = 0;
		int rankIncrement = 1;
		int lastMostSwords = 0;

		// Assign points to the layout controller.
		while (swordsUsed.Count > 0)
		{
			int playerIDWithMost = -1;
			int mostSwords = 0;

			// Find the player with the most swords.
			foreach (var playerID in swordsUsed.Keys)
			{
				if(swordsUsed[playerID].Count > mostSwords)
				{
					playerIDWithMost = playerID;
					mostSwords = swordsUsed[playerID].Count;
				}
			}

			if(mostSwords == lastMostSwords)
			{
				++rankIncrement;
			}
			else
			{
				rank += rankIncrement;
				rankIncrement = 1;
				lastMostSwords = mostSwords;
			}

			playerLayout.SetPlayerStats(resultID++, rank, playerIDWithMost, mostSwords);
			swordsUsed.Remove(playerIDWithMost);

			yield return null;
		}
		
		yield return new WaitForSeconds(5.0f);
		StartCoroutine(FadeOut());
	}

	// Fade the screen to black and load the start screen.
	private IEnumerator FadeOut()
	{
		Connections.RemoveAllInputs();

		for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime)
		{
			backgroundColour.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - t);

			yield return null;
		}

		PointTracker.instance.EndGame();
		SceneManager.LoadScene("sc_ConnectScene");
	}
}
