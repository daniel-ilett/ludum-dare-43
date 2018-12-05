using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectionController : MonoBehaviour
{
	// Message that tells players to press JUMP.
	[SerializeField]
	private Text headerText;

	// White bar that fills across the screen when everyone is ready.
	[SerializeField]
	private Image headerBG;

	// A persistent input controller that controls a player in the next scene.
	[SerializeField]
	private ConnectedInput inputPrefab;
	
	[SerializeField]
	private InputRemap remapperPrefab;

	[SerializeField]
	private Transform remapperRoot;

	private List<InputRemap> remappers;

	// Keep track of which inputs are connected.
	private List<int> connectedControllerIDs;
	private bool keyboardConnected;

	private int playersConnected = 0;
	private bool isLoadingGame = false;

	private Coroutine loadingRoutine;

	private const int maxPlayerCount = 8;

	private void Awake()
	{
		connectedControllerIDs = new List<int>();
		remappers = new List<InputRemap>();

		for(int i = 0; i < maxPlayerCount; ++i)
		{
			var newRemapper = Instantiate(remapperPrefab, remapperRoot);
			newRemapper.SetPlayerNumber(i + 1);

			remappers.Add(newRemapper);
		}
	}

	private void Update()
	{
		/*
		// Loop through all joysticks.
		for(int js = 1; js <= 16; ++js)
		{
			// Loop through all buttons.
			for(int b = 0; b < 20; ++b)
			{
				if(Input.GetKeyDown("joystick " + js + " button " + b))
				{
					Debug.Log("joystick " + js + " button " + b);
				}
			}

			for(int a = 0; a < 20; ++a)
			{
				if (Mathf.Abs(Input.GetAxis("joystick " + js + " axis " + a)) > 0.5f)
				{
					Debug.Log("joystick " + js + " axis " + a);
				}
			}
		}
		*/

		// If a connected player presses Jump, they are ready to play.
		foreach (var connectedInput in Connections.connectedInputs)
		{
			if (connectedInput.PressedJump())
			{
				if(!connectedInput.remapper.TogglePlayerReady())
				{
					StopLoading();
				}
			}
		}
		
		// Attempt to connect new controllers.
		if (playersConnected < maxPlayerCount)
		{
			if (Input.GetButtonDown("K_Jump"))
			{
				AddKeyboard();
			}

			// Poll controllers for jump nbutton.
			for (int i = 1; i <= 8; ++i)
			{
				if (Input.GetButtonDown("J" + i + "_Jump"))
				{
					AddController(i);
				}
			}
		}

		// After polling, check again if all players are ready.
		// If all are ready, start the loading coroutine.
		if(loadingRoutine == null && playersConnected > 0)
		{
			foreach (var connectedInput in Connections.connectedInputs)
			{
				if (connectedInput.remapper.IsWaiting())
				{
					return;
				}
			}

			loadingRoutine = StartCoroutine(LoadGame());
		}
	}

	// Load the game with a nice countdown.
	private IEnumerator LoadGame()
	{
		isLoadingGame = true;

		float count = 4.0f;

		while(count > 1.0f)
		{
			headerText.text = "Everyone is ready - starting in " + (int)count + "...";
			headerBG.fillAmount = 1.0f - ((count - 1.0f) / 3.0f);

			count -= Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene("sc_GameScene");
	}

	// Add the keyboard if not already added.
	private void AddKeyboard()
	{
		if(!keyboardConnected)
		{
			var input = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
			input.SetConnType(ConnectionType.KEYBOARD, 0, remappers[playersConnected], ++playersConnected);
			remappers[playersConnected - 1].SetPlayerConnected();
			
			Connections.AddInput(input);

			keyboardConnected = true;
			StopLoading();
		}
	}

	// Add a controller if not already added.
	private void AddController(int joystickID)
	{
		if (!connectedControllerIDs.Contains(joystickID))
		{
			var input = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
			input.SetConnType(ConnectionType.CONTROLLER, joystickID, remappers[playersConnected], ++playersConnected);
			remappers[playersConnected - 1].SetPlayerConnected();
			
			Connections.AddInput(input);

			connectedControllerIDs.Add(joystickID);
			StopLoading();
		}
	}

	// Stop trying to enter the scene.
	private void StopLoading()
	{
		if(loadingRoutine != null)
		{
			headerText.text = "Press JUMP to join or be ready";
			headerBG.fillAmount = 0.0f;

			StopCoroutine(loadingRoutine);
			loadingRoutine = null;
		}
	}
}
