using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
	[SerializeField]
	private Shader blurShader;

	private float blurAmount = 0.0f;
	private Material mat;

	private new Camera camera;

	private const float increaseRate = 0.005f;

	private void Awake()
	{
		camera = GetComponent<Camera>();

		// Create the material to use for the effect.
		mat = new Material(blurShader);
		mat.SetFloat("_Strength", blurAmount);
	}

	// Render the chromatic effect on the screen where necessary.
	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if(blurAmount > 0.0f)
		{
			Graphics.Blit(src, dst, mat);
		}
		else
		{
			Graphics.Blit(src, dst);
		}
	}

	// Begin overtime preceedings - turn the background black.
	public void StartOvertime()
	{
		StartCoroutine(TurnBGBlack());
	}

	// Turn the screen to black quickly.
	private IEnumerator TurnBGBlack()
	{
		for(float t = 0.25f; t > 0.0f; t -= Time.unscaledDeltaTime)
		{
			var col = t * 4.0f;
			camera.backgroundColor = new Color(col, col, col, 1.0f);
			yield return null;
		}

		camera.backgroundColor = Color.black;
	}

	// Begin increasing the strength.
	public void StartBlur()
	{
		StartCoroutine(IncreaseStrength());
	}

	// Increase the chomatic effect strength indefinitely.
	private IEnumerator IncreaseStrength()
	{
		while (true)
		{
			blurAmount += Time.unscaledDeltaTime * increaseRate;
			mat.SetFloat("_Strength", blurAmount);

			yield return null;
		}
	}
}
