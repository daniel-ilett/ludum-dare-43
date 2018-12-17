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
	private Transform playerHolder;

	[SerializeField]
	private ResultsLayout layoutEight;

	[SerializeField]
	private ResultsLayout layoutFour;

	private ResultsLayout currentLayout;

	private void Awake()
	{
		StartCoroutine(CountScores());
	}

	// Count and animate the scores for each player.
	private IEnumerator CountScores()
	{
		var swordsUsed = PointTracker.instance.GetPointSwords();

		// Enable the correct layout for the number of players.
		if(swordsUsed.Count > 4)
		{
			layoutEight.gameObject.SetActive(true);
			layoutFour.gameObject.SetActive(false);

			currentLayout = layoutEight;
		}
		else
		{
			layoutEight.gameObject.SetActive(false);
			layoutFour.gameObject.SetActive(true);

			currentLayout = layoutFour;
		}

		// Assign points to the layout controller.
		foreach(var playerID in swordsUsed.Keys)
		{
			currentLayout.SetPlayerStats(playerID, swordsUsed[playerID].Count);
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
