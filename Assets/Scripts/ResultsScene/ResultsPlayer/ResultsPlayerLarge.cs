using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsPlayerLarge : ResultsPlayer
{
	[SerializeField]
	private Transform swordImageLayout;

	[SerializeField]
	private GameObject swordPrefab;

	[SerializeField]
	private GameObject bigSwordPrefab;

	// Set the sword total counter and give a graphical representation.
	public override void SetSwordCount(int swordCount)
	{
		scoreText.text = swordCount.ToString() + " swords";

		int tens = Mathf.FloorToInt(swordCount / 10);
		swordCount -= tens * 10;
		
		for(int i = 0; i < tens; ++i)
		{
			Instantiate(bigSwordPrefab, swordImageLayout);
		}

		for(int i = 0; i < swordCount; ++i)
		{
			Instantiate(swordPrefab, swordImageLayout);
		}
	}
}
