using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsController : MonoBehaviour
{
	[SerializeField]
	private Image backgroundColour;

	private void Awake()
	{
		StartCoroutine(FadeOut());
	}

	// Fade the screen to black and load the start screen.
	private IEnumerator FadeOut()
	{
		Connections.RemoveAllInputs();

		for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime)
		{
			backgroundColour.color = new Color(t, t, t, 1.0f);

			yield return null;
		}

		SceneManager.LoadScene("sc_ConnectScene");
	}
}
