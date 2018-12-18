using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*	ResultsPlayer is a representation of one player on the Results screen. It
 *	tells the players how well a given player scored, as well as what position
 *	they came in and which character they were playing as.
 */
public class ResultsPlayer : MonoBehaviour
{
	[SerializeField]
	private Text rankText;

	[SerializeField]
	protected Text playerIDText;

	[SerializeField]
	protected Text scoreText;

	[SerializeField]
	private Transform swordImageLayout;

	[SerializeField]
	private GameObject swordPrefab;

	[SerializeField]
	private GameObject bigSwordPrefab;

	private RectTransform rectTransform;

	private static Dictionary<int, string> rankNames = new Dictionary<int, string>
	{
		{ 1, "1st" },
		{ 2, "2nd" },
		{ 3, "3rd" },
		{ 4, "4th" },
		{ 5, "5th" },
		{ 6, "6th" },
		{ 7, "7th" },
		{ 8, "8th" }
	};

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	// Set the rank, playerID, and sword count.
	public void SetStats(int rank, int playerID, int points)
	{
		rankText.text = rankNames[rank];
		playerIDText.text = "P" + playerID.ToString();
		SetSwordCount(points);
	}

	// Set the sword total counter and give a graphical representation.
	public void SetSwordCount(int swordCount)
	{
		scoreText.text = swordCount.ToString() + ((swordCount != 1) ? " swords" : " sword");

		int tens = Mathf.FloorToInt(swordCount / 10);
		swordCount -= tens * 10;

		for (int i = 0; i < tens; ++i)
		{
			Instantiate(bigSwordPrefab, swordImageLayout);
		}

		for (int i = 0; i < swordCount; ++i)
		{
			Instantiate(swordPrefab, swordImageLayout);
		}
	}
}
