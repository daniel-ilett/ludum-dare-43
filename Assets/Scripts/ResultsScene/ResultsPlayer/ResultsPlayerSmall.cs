using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsPlayerSmall : ResultsPlayer
{
	// Only set the sword counter, nothing special.
	public override void SetSwordCount(int swordCount)
	{
		scoreText.text = swordCount.ToString() + " swords";
	}
}
