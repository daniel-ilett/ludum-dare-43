using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
	[SerializeField]
	private SwordEntity swordPrefab;

	[SerializeField]
	private SpriteRenderer heldSwordPrefab;

	public static SwordManager instance { get; private set; }

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	public SwordEntity CreateSword(Vector3 position, Quaternion rotation)
	{
		return Instantiate(swordPrefab, position, rotation);
	}

	public SpriteRenderer CreateHeldSword(Transform parent, Quaternion rotation)
	{
		return Instantiate(heldSwordPrefab, parent.position, rotation, parent);
	}
}
