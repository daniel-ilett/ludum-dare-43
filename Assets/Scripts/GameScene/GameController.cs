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

	[SerializeField]
	private GameCamera gameCamera;

	private GameTimer timer;

	private void Awake()
	{
		timer = GetComponent<GameTimer>();

		// Subscribe to game end event.
		timer.TimerExpired += GameEnded;

		var inputs = Connections.connectedInputs;
		
		for(int i = 0; i < inputs.Count; ++i)
		{
			var input = inputs[i];
			var newPlayer = Instantiate(playerPrefab, spawnPositions[i].position, Quaternion.identity);

			newPlayer.connectedInput = input;
		}
	}

	// Called whenever the game timer expires.
	private void GameEnded(object sender, EventArgs e)
	{
		Debug.Log("Game Over");

		StartCoroutine(EndGame());
	}

	// We may need to wait on an overtime segment. Else, load the results screen.
	private IEnumerator EndGame()
	{
		if(!PointTracker.instance.IsAWinner())
		{
			yield return StartCoroutine(RunOvertime());
		}

		gameCamera.StartBlur();
		StartCoroutine(LoadResults());
	}

	// Start overtime prceedings.
	private IEnumerator RunOvertime()
	{
		// Change text to overtime.
		timer.StartOvertime();

		gameCamera.StartOvertime();

		while(!PointTracker.instance.IsAWinner())
		{
			yield return null;
		}
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
