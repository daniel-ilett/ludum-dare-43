using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameTimer))]
public class GameController : MonoBehaviour
{
	[SerializeField]
	private PlayerController playerPrefab;

	[SerializeField]
	private List<Transform> spawnPositions;

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

	// 
	private void GameEnded(object sender, EventArgs e)
	{
		Debug.Log("Game Over");
	}
}
