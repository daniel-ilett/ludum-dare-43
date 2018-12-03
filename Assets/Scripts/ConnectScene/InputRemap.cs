using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputRemap : MonoBehaviour
{
	[SerializeField]
	private Image bgImage;

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

		StartCoroutine(LightenBackground());
	}

	// Raise a white bar over the remapper when the player connects.
	private IEnumerator LightenBackground()
	{
		for(float t = 0.0f; t < 0.5f; t += Time.deltaTime)
		{
			bgImage.fillAmount = t / 0.5f;
			yield return null;
		}

		bgImage.fillAmount = 1.0f;
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
