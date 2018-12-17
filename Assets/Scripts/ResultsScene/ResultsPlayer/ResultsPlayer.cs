using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ResultsPlayer : MonoBehaviour
{
	[SerializeField]
	protected Text playerIDText;

	[SerializeField]
	protected Text scoreText;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	// Set the playerID and sword count.
	public void SetStats(int playerID, int points)
	{
		SetActive(true);
		SetPlayerID(playerID);
		SetSwordCount(points);
	}

	// Change the playerID text.
	private void SetPlayerID(int playerID)
	{
		playerIDText.text = "P" + playerID.ToString();
	}

	// Set the ResultsPlayer and its parent actve or inactive accordingly.
	public void SetActive(bool isActive)
	{
		Debug.Log("Set active...");
		transform.parent.gameObject.SetActive(isActive);
	}

	// Set the number of swords this player threw.
	public abstract void SetSwordCount(int swordCount);
}
