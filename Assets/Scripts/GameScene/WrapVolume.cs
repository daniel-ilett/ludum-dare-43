using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapVolume : MonoBehaviour
{
	[SerializeField]
	private WrapVolume otherVolume;

	[SerializeField]
	private Vector2 spawnOffset;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			var player = other.GetComponent<PlayerController>();

			if(player != null)
			{
				otherVolume.SpawnAtOffset(player);
			}
		}
	}

	// Move the player to this volume's safe spawn position.
	private void SpawnAtOffset(PlayerController player)
	{
		player.SetXPosition(transform.position + (Vector3)spawnOffset);
	}
}
