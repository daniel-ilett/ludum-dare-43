using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*	ResultsController handles the flow of events on the Results screen. First of
 *	all, the results are displayed on screen during a fade-in effect. Then,
 *	we wait until the game is ready to reset. We fade out to black and the
 *	connection screen is displayed.
 */
public class ResultsController : MonoBehaviour
{
	[SerializeField]
	private Image backgroundColour;

	[SerializeField]
	private ResultsLayout playerLayout;
	
	private void Awake()
	{
		StartCoroutine(FadeIn());
		StartCoroutine(CountScores());
	}

	// Count and animate the scores for each player.
	private IEnumerator CountScores()
	{
		var swordsUsed = PointTracker.instance.GetPointSwords();

		playerLayout.LayoutResults(swordsUsed.Count);

		int resultID = 0;

		int rank = 0;
		int rankIncrement = 1;
		int lastMostSwords = 0;

		// Assign points to the layout controller.
		while (swordsUsed.Count > 0)
		{
			int playerIDWithMost = -1;
			int mostSwords = -1;

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
		
		yield return new WaitForSeconds(10.0f);
		StartCoroutine(FadeOut());
	}

	// Fade the screen in from white to transparent.
	private IEnumerator FadeIn()
	{
		for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime)
		{
			backgroundColour.color = new Color(1.0f, 1.0f, 1.0f, t);

			yield return null;
		}

		backgroundColour.color = Color.clear;
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
