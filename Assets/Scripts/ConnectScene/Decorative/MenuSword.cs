using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSword : MonoBehaviour
{
	[SerializeField]
	private Transform gimbal;

	[SerializeField]
	private Transform model;

	private void Update()
	{
		gimbal.Rotate(Time.deltaTime * 5.0f, 0.0f, 0.0f);
		model.Rotate(0.0f, Time.deltaTime * 5.0f, 0.0f);
	}
}
