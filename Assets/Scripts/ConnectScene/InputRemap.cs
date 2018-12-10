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

	private bool isJoined = false;
	private bool isReady = false;

	private void Start()
	{
		joinOrReadyText.text = "Join";
		playerSprite.SetActive(false);
	}

	// Let the remapper know a player has connected.
	public void SetPlayerConnected()
	{
		isJoined = true;

		playerSprite.SetActive(true);
		joinOrReadyText.text = "Not\nReady";

		StartCoroutine(LightenBackground());
	}

	// Raise a white bar over the remapper when the player connects.
	private IEnumerator LightenBackground()
	{
		for(float t = 0.0f; t < 0.25f; t += Time.deltaTime)
		{
			bgImage.fillAmount = t / 0.25f;
			yield return null;
		}

		bgImage.fillAmount = 1.0f;
	}

	// Return whether this player is connected but isn't ready.
	public bool IsWaiting()
	{
		return (isJoined && !isReady);
	}

	// Set that this player is ready to start.
	public void SetPlayerReady(bool isReady)
	{
		this.isReady = isReady;

		joinOrReadyText.text = isReady ? "Ready!" : "Not\nReady";
	}

	// Switch the player ready state.
	public bool TogglePlayerReady()
	{
		SetPlayerReady(!isReady);

		return isReady;
	}

	// Set the text that appears for this remapper.
	public void SetPlayerNumber(int playerID)
	{
		playerNumberText.text = "P" + playerID;
	}
}
