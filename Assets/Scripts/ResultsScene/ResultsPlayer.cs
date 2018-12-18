using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	// Set the rank, playerID, and sword count.
	public void SetStats(int rank, int playerID, int points)
	{
		rankText.text = "#" + rank.ToString();
		playerIDText.text = "P" + playerID.ToString();
		SetSwordCount(points);
	}

	// Set the sword total counter and give a graphical representation.
	public void SetSwordCount(int swordCount)
	{
		scoreText.text = swordCount.ToString() + " swords";

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
