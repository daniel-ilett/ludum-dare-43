using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameTimer))]
public class GameController : MonoBehaviour
{
	[SerializeField]
	private PlayerController playerPrefab;

	[SerializeField]
	private List<Transform> spawnPositions;

	[SerializeField]
	private Image backgroundColor;

	private void Awake()
	{
		// Subscribe to game end event.
		GetComponent<GameTimer>().TimerExpired += GameEnded;

		var inputs = Connections.connectedInputs;
		
		for(int i = 0; i < inputs.Count; ++i)
		{
			var input = inputs[i];
			var newPlayer = Instantiate(playerPrefab, spawnPositions[i].position, Quaternion.identity);

			newPlayer.connectedInput = input;
		}
	}

	private void Update()
	{
		
	}

	// Called whenever the game timer expires.
	private void GameEnded(object sender, EventArgs e)
	{
		Debug.Log("Game Over");
		StartCoroutine(LoadResults());
	}

	// Load the results screen.
	private IEnumerator LoadResults()
	{
		Time.timeScale = 0.0f;

		for(float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime)
		{
			backgroundColor.color = new Color(1.0f, 1.0f, 1.0f, t);
			yield return null;
		}

		Time.timeScale = 1.0f;
		SceneManager.LoadScene("sc_ResultsScene");
	}
}
