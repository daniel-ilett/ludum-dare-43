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
		var position = Vector3.zero;
		var rotation = Quaternion.identity;

		var newSword = CreateSword(position, rotation);
	}
}
