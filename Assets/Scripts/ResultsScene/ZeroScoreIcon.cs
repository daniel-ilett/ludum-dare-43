using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZeroScoreIcon : MonoBehaviour
{
	[SerializeField]
	private Text playerNumber;

	[SerializeField]
	private Image playerImage;

	// Set the player's number text.
	public void SetPlayerNumber(int playerID)
	{
		playerNumber.text = "P" + playerID.ToString();
	}
}
