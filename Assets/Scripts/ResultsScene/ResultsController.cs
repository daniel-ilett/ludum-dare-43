using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsController : MonoBehaviour
{
	[SerializeField]
	private SwordEntity swordPrefab;

	[SerializeField]
	private ResultsPlayer playerPrefab;

	[SerializeField]
	private Image backgroundColour;

	[SerializeField]
	private Transform playerHolder;

	[SerializeField]
	private Sacrifice sacrifice;

	private void Awake()
	{
		StartCoroutine(CountScores());
	}

	// Count and animate the scores for each player.
	private IEnumerator CountScores()
	{
		var swordsUsed = PointTracker.instance.GetPointSwords();

		int totalSwordCount = 0;

		// Find the total number of swords that were used.
		foreach (var playerID in swordsUsed.Keys)
		{
			totalSwordCount += swordsUsed[playerID].Count;
		}

		float increment = 210.0f / totalSwordCount;
		float currentAngle = 75.0f + (increment / 2.0f);

		float distance = 2.0f;

		bool shouldSearch = true;
		var wait = new WaitForSeconds(0.1f);

		// Find the player with most swords thrown and add their score.
		while(shouldSearch)
		{
			var mostSwords = 0;
			int playerIDWithMost = -1;

			foreach(var playerID in swordsUsed.Keys)
			{
				if(swordsUsed[playerID].Count > mostSwords)
				{
					playerIDWithMost = playerID;
					mostSwords = swordsUsed[playerID].Count;
				}
			}

			if(mostSwords == 0)
			{
				shouldSearch = false;
			}
			else
			{
				float averageRadians = 0.0f;

				float xPos;
				float yPos;

				// Throw each sword at the sacrifice.
				foreach(var swordSprite in swordsUsed[playerIDWithMost])
				{
					var radians = Mathf.Deg2Rad * currentAngle;
					averageRadians += radians;

					xPos = distance * Mathf.Sin(radians);
					yPos = distance * Mathf.Cos(radians);

					var newPos = new Vector3(xPos, yPos) + sacrifice.transform.position;

					var newSword = Instantiate(swordPrefab, newPos, Quaternion.identity);
					newSword.transform.up = sacrifice.transform.position - newPos;
					newSword.SetSprite(swordSprite);

					newSword.Throw(-1);

					currentAngle += increment;
					yield return wait;
				}

				// Find the average angle swords are placed at and add a player icon there.
				averageRadians /= mostSwords;

				xPos = distance * 1.5f * Mathf.Sin(averageRadians);
				yPos = distance * 1.5f * Mathf.Cos(averageRadians);

				var pos = new Vector3(xPos, yPos) + sacrifice.transform.position;

				var resultsPlayer = Instantiate(playerPrefab, playerHolder);
				resultsPlayer.SetPosition(pos);
				resultsPlayer.SetPlayerID(playerIDWithMost);
				resultsPlayer.SetSwordCount(mostSwords);

				Debug.Log("Player " + playerIDWithMost + " has " + mostSwords + " swords.");
				swordsUsed.Remove(playerIDWithMost);
			}
		}

		foreach(var playerID in swordsUsed.Keys)
		{
			Debug.Log("Player " + playerID + " had no swords.");
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
