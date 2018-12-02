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
	private List<GameObject> spawnPositions;

	private void Awake()
	{
		// Subscribe to game end event.
		GetComponent<GameTimer>().TimerExpired += GameEnded;
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
