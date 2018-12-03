using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoEnter : MonoBehaviour
{
	[SerializeField]
	private Image logoBGImage;

	[SerializeField]
	private Image logoImage;

	private void Start()
	{
		StartCoroutine(Enter());
	}

	// Fade the logo image in.
	private IEnumerator Enter()
	{
		var pos = logoImage.rectTransform.anchoredPosition;

		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime)
		{
			logoBGImage.fillAmount = t / 1.0f;
			logoImage.rectTransform.anchoredPosition = pos + new Vector2((1.0f - t) * 40.0f, 0.0f);
			logoImage.color = new Color(1.0f, 1.0f, 1.0f, t);
			yield return null;
		}

		// Set to final positions.
		logoBGImage.fillAmount = 1.0f;
		logoImage.rectTransform.anchoredPosition = pos;
		logoImage.color = Color.white;

		// The component is no longer needed.
		Destroy(this);
	}
}
