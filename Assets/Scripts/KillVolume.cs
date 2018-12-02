using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "Player":
				{
					// Kill players who touch the kill volume.
					var player = other.GetComponent<PlayerController>();

					if (player != null)
					{
						player.KillPlayer();
					}
				}
				break;
			case "Sword":
				{
					// Destroy swords that hit the kill volume.
					var sword = other.GetComponent<SwordEntity>();

					if(sword != null)
					{
						sword.DestroySword();
					}
				}
				break;
		}
	}
}
