using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private PlayerController playerPrefab;

	[SerializeField]
	private List<GameObject> spawnPositions;
}
