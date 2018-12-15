using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
	[SerializeField]
	private SwordEntity swordPrefab;

	[SerializeField]
	private SpriteRenderer heldSwordPrefab;

	[SerializeField]
	private List<Sprite> swordSprites;

	[SerializeField]
	private Collider2D spawnVolume;

	private bool shouldSpawn = true;
	private Coroutine spawnRoutine = null;

	public static SwordManager instance { get; private set; }

	private void Awake()
	{
		// Make this class a singleton instance of SwordManager.
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;

			spawnRoutine = StartCoroutine(SpawnLoop());
		}
	}

	// Another class my request a sword to be created.
	public SwordEntity CreateSword(Vector3 position, Quaternion rotation)
	{
		return Instantiate(swordPrefab, position, rotation);
	}

	// A PlayerController may request a sword sprite object to be created.
	public SpriteRenderer CreateHeldSword(Transform parent, Quaternion rotation)
	{
		return Instantiate(heldSwordPrefab, parent.position, rotation, parent);
	}

	// Create a sword to add to the world.
	private void SpawnSword()
	{
		var bounds = spawnVolume.bounds;
		var position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), 
			Random.Range(bounds.min.y, bounds.max.y));
		var rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f + Random.Range(-30.0f, 30.0f));

		var newSword = CreateSword(position, rotation);

		var newSprite = swordSprites[Random.Range(0, swordSprites.Count)];
		newSword.SetSprite(newSprite);
		newSword.Throw(-1);
	}

	// Maintain a loop of spawning a sword every few seconds.
	private IEnumerator SpawnLoop()
	{
		var wait = new WaitForSeconds(2.0f);

		while (shouldSpawn)
		{
			yield return wait;
			SpawnSword();
		}
	}

	// Stop the sword-spawning loop when the object is destroyed.
	private void OnDestroy()
	{
		shouldSpawn = false;
	}
}
