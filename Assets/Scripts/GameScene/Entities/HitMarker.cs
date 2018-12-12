using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitMarker : MonoBehaviour
{
	private new ParticleSystem particleSystem;

	private void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}

	// Play the particle effect.
	public void Play(Vector3 position)
	{
		transform.position = position;
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 90.0f));
		particleSystem.Play();
	}

	// Destroy the particles after they have stopped playing.
	public void DestroyAfterPlaying()
	{
		var time = (particleSystem.isPlaying) ? particleSystem.main.duration : 0.0f;

		Invoke("DestroyNow", time);
	}

	// Destroy the particles now.
	private void DestroyNow()
	{
		Destroy(gameObject);
	}
}
