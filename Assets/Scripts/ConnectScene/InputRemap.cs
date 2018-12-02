using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputRemap : MonoBehaviour
{
	[SerializeField]
	private Text playerNumberText;

	[SerializeField]
	private Text joinOrReadyText;

	[SerializeField]
	private GameObject playerSprite;

	private void Start()
	{
		joinOrReadyText.text = "Join";
		playerSprite.SetActive(false);
	}

	// Let the remapper know a player has connected.
	public void SetPlayerConnected()
	{
		playerSprite.SetActive(true);
		joinOrReadyText.text = "Not\nReady";
	}

	// Set that this player is ready to start.
	public void SetPlayerReady()
	{
		joinOrReadyText.text = "Ready!";
	}

	// Set the text that appears for this remapper.
	public void SetPlayerNumber(int playerID)
	{
		playerNumberText.text = "P" + playerID;
	}
}
