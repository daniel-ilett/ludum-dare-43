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

	[SerializeField]
	private Image fadePlane;

	// A persistent input controller that controls a player in the next scene.
	[SerializeField]
	private ConnectedInput inputPrefab;
	
	[SerializeField]
	private InputRemap remapperPrefab;

	[SerializeField]
	private Transform remapperRoot;

	private List<InputRemap> remappers;

	private bool isLoadingGame = false;
	private bool hasLoadedGame = false;

	private Coroutine loadingRoutine;

	public const int maxPlayerCount = 8;

	private void Awake()
	{
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
		if(!hasLoadedGame)
		{
			PollControllers();
		}
	}

	private void PollControllers()
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

		List<ConnectedInput> inputsToRemove = new List<ConnectedInput>();

		foreach (var connectedInput in Connections.connectedInputs)
		{
			// If a connected player presses Jump, they are ready to play.
			if (connectedInput.PressedJump())
			{
				if(!connectedInput.remapper.TogglePlayerReady())
				{
					StopLoading();
				}
			}

			// If a connected player presses back, they are not ready or they
			// wish to disconnect.
			if(connectedInput.PressedBack())
			{
				if(connectedInput.remapper.IsWaiting())
				{
					inputsToRemove.Add(connectedInput);
				}
				else
				{
					if (!connectedInput.remapper.TogglePlayerReady())
					{
						StopLoading();
					}
				}
			}
		}

		// Remove all ConnectedInputs that backed out completely.
		foreach(var inputToRemove in inputsToRemove)
		{
			RemoveController(inputToRemove.GetJoystickID());
		}
		
		// Attempt to connect new controllers.
		if (Connections.connectedInputs.Count < maxPlayerCount)
		{
			if (Input.GetButtonDown("K_Jump"))
			{
				AddController(0);
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
		if(loadingRoutine == null && Connections.connectedInputs.Count > 0)
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

		hasLoadedGame = true;
		yield return StartCoroutine(FadeToWhite());

		SceneManager.LoadScene("sc_GameScene");
	}

	// Fade the screen to white.
	private IEnumerator FadeToWhite()
	{
		float count = 0.5f;

		while(count > 0.0f)
		{
			fadePlane.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (count / 0.5f));

			count -= Time.deltaTime;
			yield return null;
		}
	}

	// Add a controller if not already added.
	private void AddController(int joystickID)
	{
		if (!Connections.IsControllerConnected(joystickID))
		{
			int newPlayerID = Connections.GetNextPlayerID();
			var remapper = remappers[newPlayerID - 1];

			// Create a persistent connection object.
			var input = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
			input.SetConnType(joystickID, remapper, newPlayerID);

			remapper.SetPlayerConnected(true);
			
			Connections.AddInput(input);
			StopLoading();
		}
	}

	// Remove a controller and destroy its 
	private void RemoveController(int joystickID)
	{
		var connectedInput = Connections.GetInputWithJoystickID(joystickID);

		if(connectedInput != null)
		{
			connectedInput.remapper.SetPlayerConnected(false);

			Connections.RemoveInput(connectedInput);
			Destroy(connectedInput);
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
