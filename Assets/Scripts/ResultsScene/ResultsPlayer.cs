using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPlayer : MonoBehaviour
{
	[SerializeField]
	private Text playerIDText;

	[SerializeField]
	private Text scoreText;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	// Set world-space position of UI element.
	public void SetPosition(Vector2 position)
	{
		var pos = Camera.main.WorldToViewportPoint(position);

		rectTransform.anchorMin = pos;
		rectTransform.anchorMax = pos;
	}

	// Change the playerID text.
	public void SetPlayerID(int playerID)
	{
		playerIDText.text = "P" + playerID.ToString();
	}

	// Set the number of swords this player threw.
	public void SetSwordCount(int swordCount)
	{
		scoreText.text = swordCount.ToString() + "pts";
	}
}
