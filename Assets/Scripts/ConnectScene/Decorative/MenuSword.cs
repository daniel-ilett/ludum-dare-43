using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	MenuSword is a decorative item that appears on the Connection screen. It
 *	has an edge-rendering shader with a thick outline for a striking effect.
 */
public class MenuSword : MonoBehaviour
{
	[SerializeField]
	private Transform gimbal;

	[SerializeField]
	private Transform model;

	[SerializeField]
	private List<GameObject> models;

	// Place a random sword on the screen.
	private void Awake()
	{
		int modelIndex = Random.Range(0, models.Count);

		Instantiate(models[modelIndex], model);
	}

	private void Update()
	{
		gimbal.Rotate(Time.deltaTime * 5.0f, 0.0f, 0.0f);
		model.Rotate(0.0f, Time.deltaTime * 5.0f, 0.0f);
	}
}
